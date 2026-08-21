# Database Documentation (LyceumSupportDesk)

This document outlines the schema design, primary keys, foreign keys, and constraints for `lycevm.db`.

---

## 1. Core Organizational Tables & Primary Keys

| Table Name | Primary Key Column(s) | Key Type | Auto-Increment |
| :--- | :--- | :--- | :--- |
| **Departments** | `Id` | Single | Yes |
| **Employees** | `Id` | Single | Yes |
| **Teams** | `Id` | Single | Yes |
| **TeamMembers** | (`TeamId`, `EmployeeId`) | Composite | No |

---

## 2. Foreign Key Relationships (Core Tables)

| Source Table | Foreign Key Column | Target Table | Target Column | Relationship Type |
| :--- | :--- | :--- | :--- | :--- |
| **Employees** | `DepartmentId` | `Departments` | `Id` | One-to-Many (Required) |
| **Teams** | `DepartmentId` | `Departments` | `Id` | One-to-Many (Required) |
| **TeamMembers** | `TeamId` | `Teams` | `Id` | Many-to-Many Junction |
| **TeamMembers** | `EmployeeId` | `Employees` | `Id` | Many-to-Many Junction |

---

## 3. Constraints & Unique Indexes

* **`Departments.Name`**: Must be unique.
* **`Employees.Email`**: Must be unique.
* **`Teams` (`DepartmentId`, `Name`)**: Composite unique constraint preventing duplicate team names within the same department.
