# 🛰️ Real-Time CDC & Streaming Project with Kafka, Debezium & .NET

A simple distributed system that demonstrates **Change Data Capture (CDC)** using **Debezium**, **Apache Kafka**, and **.NET** to propagate changes from a primary (writer) PostgreSQL database to a read-optimized replica. The project is fully containerized with Docker Compose and implements a straightforward **CQRS** pattern.

---

## ✅ Features

- 🔄 **Change Data Capture** using Debezium monitoring PostgreSQL WAL logs
- ⚡ **Real-time streaming** of database changes via Apache Kafka
- 🧱 **CQRS** separation of write and read databases
- 🛠️ .NET Worker Service as Kafka consumer for asynchronous processing
- 🐳 Fully containerized with Docker Compose
- 📡 **Kafka UI** dashboard for monitoring topics on port 9000

---

## 📐 Architecture Overview

This flow illustrates how a user request travels through the system:

```text
[User]
   │
   ▼
[Web API Service]       # Receives HTTP/HTTPS request
   │
   ▼
[PostgreSQL (Writer)]   # Executes write/read operations
   │
   ▼
[Debezium]              # Monitors WAL and publishes CDC events
   │
   ▼
[Apache Kafka]          # Streams CDC events to topics
   │
   ▼
[.NET Worker Service]   # Consumes events and updates replica
   │
   ▼
[PostgreSQL (Read)]     # Read-optimized database for queries
```

---

[![View Architecture Diagram](https://img.shields.io/badge/View_Diagram-PDF-blue)](./figjam-diagram.pdf)

---

## 🧰 Technologies Used

| Layer                | Technology                        |
| -------------------- | --------------------------------- |
| CDC Engine           | Debezium (Postgres Connector)     |
| Message Broker       | Apache Kafka (Confluent Platform) |
| Kafka Client         | Confluent.Kafka (.NET)            |
| API Service          | ASP.NET Core Web API              |
| Background Worker    | .NET Worker Service               |
| Databases            | PostgreSQL                        |
| Orchestration        | Docker, Docker Compose            |
| Monitoring Dashboard | Kafka UI                          |
| Serialization        | System.Text.Json                  |

---

## 📁 Project Structure

```text
.
├── WebApi/                   # ASP.NET Core Web API project
├── WorkerService/            # .NET Worker Service (Kafka consumer)
├── docker-compose.yml        # Docker Compose orchestration (includes sample .env)
├── .env                      # Sample environment variables preconfigured
├── scripts/                  # Utility scripts (e.g., DB setup)
├── README.md                 # Project documentation
└── LICENSE                   # MIT license file
```

---

## 🛠️ Prerequisites

- **Docker**
- **Docker Compose**
- **Git**

---

## 🚀 Getting Started

clone the repository using the command

```bash
git clone https://github.com/Roger-css/Replication-with-streaming.git
```

Navigate to the directory

```bash
cd Replication-with-streaming
```

Launch all components with a single command:

```bash
docker-compose up --build
```

This starts:

- PostgreSQL (writer)
- Debezium (with Kafka Connect)
- Zookeeper & Kafka
- Kafka UI (port 9000)
- Web API (HTTP on 8080, HTTPS on 8081)
- PGAdmin (port 5050)
- Worker Service

Access endpoints:

- Kafka UI: `http://localhost:9000`
- Web API: `http://localhost:8080` or `https://localhost:8081`
- PGAdmin: `http://localhost:5050`

---

## 👨‍💻 Author

**Mustafa** – Full Stack Software Engineer exploring distributed systems and real-time data replication.

Feel free to open issues or pull requests for enhancements!
