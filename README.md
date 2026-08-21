# LyceumSupportDesk

**LyceumSupportDesk** is a comprehensive, enterprise-grade IT and customer support ticket management system built using **ASP.NET Core MVC** and **Entity Framework Core**. Designed specifically for streamlined helpdesk operations, it features robust employee workload tracking, collaborative multi-assignee workflows, hierarchical category mapping, and internal communication logs.

---

## 🚀 Key Features

* **Customer & Employee Directory:** Complete CRUD operations for managing customer accounts, corporate entities, and internal staff members.
* **Workload Analytics:** Real-time metrics tracking active ticket distribution across departments and individual staff members.
* **Hierarchical Ticket Categories:** Self-referencing parent-child category architecture (e.g., *Hardware $\rightarrow$ Peripherals*) to cleanly organize support topics.
* **Unassigned Tickets Queue:** Dedicated dispatch queue allowing supervisors to isolate unhandled requests and assign staff instantly.
* **Collaborative Multi-Assignee Workflow:** Support for assigning multiple staff members to complex or high-priority tickets simultaneously.
* **Internal Notes & Activity Feed:** Time-stamped internal logging system enabling technicians to document troubleshooting steps and remarks securely.
* **System Tagging & Reports:** Comprehensive tag management and structural category hierarchy reports for administrative insights.

---

## 🛠️ Technology Stack

* **Framework:** ASP.NET Core MVC (.NET 8.0)
* **ORM:** Entity Framework Core (Manual Code-First modeling with Fluent API relationships)
* **Database:** Local Database Storage (SQLite / SQL Server compatible via Entity Framework Migrations)
* **Frontend:** Bootstrap 5, Bootstrap Icons, and custom styling (`navbar-loa` theme)

---

## ⚙️ Getting Started & Local Installation

### Prerequisites
Ensure you have the following installed on your development machine:
* [.NET SDK](https://dotnet.microsoft.com/) (version 8.0 or higher recommended)
* An IDE or code editor such as **Visual Studio** or **Visual Studio Code**

### Installation Steps

1. **Clone or Download the Repository:**
   ```bash
   git clone [https://github.com/your-username/LyceumSupportDesk.git](https://github.com/your-username/LyceumSupportDesk.git)
   cd LyceumSupportDesk
