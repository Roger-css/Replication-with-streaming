# 🛰️ Real-Time Distributed Messaging System with Kafka, Debezium & .NET

A real-time distributed system that leverages **Change Data Capture (CDC)** via **Debezium**, **Apache Kafka**, and **.NET** to propagate changes from a primary (writer) PostgreSQL database to a read-optimized replica. This project is fully containerized and follows CQRS-style architecture for production-grade scalability and reliability.

---

## 📦 Badges

&#x20;&#x20;

---

## ✅ Features

- 🔄 **Change Data Capture** using Debezium monitoring PostgreSQL WAL logs
- ⚡ **Real-time streaming** of database changes via Apache Kafka
- 🧱 **CQRS-style** separation of write and read databases
- 🛠️ **.NET Worker Service** as Kafka consumer for asynchronous processing
- 🐳 **Docker Compose** for service orchestration
- 📡 **Kafka UI** dashboard for monitoring topics and events
- 🔐 **Secure** configuration with environment variables and secrets

---

## 📐 Architecture Overview

```text
+----------------------+
| PostgreSQL (Writer)  |
+----------------------+
           │
           ▼
+----------------------+
|     Debezium         |  <- Monitors WAL, publishes CDC events
+----------------------+
           │
           ▼
+----------------------+
|      Kafka           |  <- Streams CDC events to topics
+----------------------+
           │
           ▼
+--------------------------------+
| .NET Kafka Consumer Service    |  <- Subscribes and processes events
+--------------------------------+
           │
           ▼
+-------------------------+
| PostgreSQL (Read Side)  |
+-------------------------+
```

---

## 🧰 Technologies Used

| Layer                | Technology                         |
| -------------------- | ---------------------------------- |
| CDC Engine           | Debezium (Postgres Connector)      |
| Message Broker       | Apache Kafka (Confluent Platform)  |
| Kafka Client         | Confluent.Kafka (.NET)             |
| API Service          | ASP.NET Core Web API (C# / .NET 8) |
| Background Worker    | .NET Worker Service (C# / .NET 8)  |
| Databases            | PostgreSQL (Writer & Replica)      |
| Orchestration        | Docker, Docker Compose             |
| Monitoring Dashboard | Kafka UI                           |
| Serialization        | System.Text.Json                   |

---

## 📁 Project Structure

```text
.
├── WebApi/                   # ASP.NET Core Web API project
├── WorkerService/            # .NET Worker Service (Kafka consumer)
├── docker-compose.yml        # Docker Compose orchestration
├── .env                      # Environment variables (optional)
├── scripts/                  # Utility scripts (e.g., DB setup)
├── .github/workflows/ci.yml  # CI pipeline config
├── README.md                 # Project documentation
└── LICENSE                   # MIT license file
```

---

## 🛠️ Prerequisites

- **Docker** (v20+)
- **Docker Compose** (v1.29+)
- **.NET 8 SDK**
- **Git**

---

## 🚀 Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/your-username/distributed-cdc-kafka.git
cd distributed-cdc-kafka
```

### 2. Configure environment variables

Copy `.env.example` to `.env` and adjust as needed:

```ini
# .env
KAFKA_BOOTSTRAP_SERVERS=localhost:9092
KAFKA_GROUP_ID=cdc-consumer-group
DB_WRITER_HOST=writer_postgres
DB_WRITER_NAME=appdb
DB_WRITER_USER=postgres
DB_WRITER_PASS=postgres
DB_REPLICA_HOST=read_replica
DB_REPLICA_NAME=appdb_read
DB_REPLICA_USER=postgres
DB_REPLICA_PASS=postgres
```

### 3. Start services

```bash
docker-compose up --build -d
```

This command launches:

- PostgreSQL (writer)
- Debezium (with Kafka Connect)
- Zookeeper & Kafka
- Kafka UI (port 8080)

### 4. Run backend services

In separate terminals:

```bash
cd WebApi
dotnet run
```

```bash
cd WorkerService
dotnet run
```

---

## 🔍 Kafka Topics & Schema

- **Topic naming**: `dbserver1.<schema>.<table>` (default Debezium pattern)
- **Serialization**: JSON payload containing `before`, `after`, and metadata

Example topic:

```
dbserver1.public.users
```

Use Kafka UI to inspect messages:

```
http://localhost:8080
```

---

## ⚙️ Configuration

Key settings available in `appsettings.json` or via environment variables:

```json
"Kafka": {
  "BootstrapServers": "${KAFKA_BOOTSTRAP_SERVERS}",
  "GroupId": "${KAFKA_GROUP_ID}",
  "AutoOffsetReset": "Earliest"
},
"ConnectionStrings": {
  "WriterDb": "Host=${DB_WRITER_HOST};Database=${DB_WRITER_NAME};Username=${DB_WRITER_USER};Password=${DB_WRITER_PASS}",
  "ReplicaDb": "Host=${DB_REPLICA_HOST};Database=${DB_REPLICA_NAME};Username=${DB_REPLICA_USER};Password=${DB_REPLICA_PASS}"
}
```

---

## 🔐 Security Considerations

- Enable TLS/SSL between Kafka brokers and clients
- Secure Debezium connectors with API authentication
- Use Docker secrets or vaults for sensitive credentials
- Configure network policies to restrict access
- Implement consumer retries and dead-letter topics

---

## 📊 Observability & Monitoring

- **Kafka UI**: `http://localhost:8080` to monitor topics and consumer groups
- **Logs**: .NET services output structured logs (console / file)
- **Metrics (Future)**: integrate Prometheus + Grafana

---

## 🧪 Testing & Validation

1. **Insert data** into writer DB:
   ```sql
   INSERT INTO users (id, name) VALUES (1, 'Alice');
   ```
2. **Debezium** captures change and publishes to Kafka
3. **Worker Service** subscribes and writes to replica DB
4. **Verify** replica state:
   ```sql
   SELECT * FROM users;  -- Should show Alice
   ```

---

## 🔄 CI/CD Pipeline

Configured in `.github/workflows/ci.yml`:

- Build and test .NET projects
- Build Docker images
- Push to Docker Hub on tag

---

## 📜 License

This project is licensed under the [MIT License](LICENSE).

---

## 📝 Roadmap & TODOs

-

---

## 👨‍💻 Author

**Mustafa** – Full Stack Software Engineer passionate about distributed systems and real-time data architectures.

Feel free to open issues or pull requests for improvements!

