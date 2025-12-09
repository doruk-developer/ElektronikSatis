# 🛍️ Doruk Shop - Modern E-Commerce Platform

![Project Status](https://img.shields.io/badge/Status-Completed-success)
![Framework](https://img.shields.io/badge/.NET%20Framework-4.7.2-purple)
![Architecture](https://img.shields.io/badge/Architecture-Server--Side%20MVC%20with%20Client--Side%20Enhancement-blue)
![Language](https://img.shields.io/badge/Interface-Turkish-red)

**Doruk Shop** is a modern and dynamic E-Commerce application built with **ASP.NET MVC**. It features a custom-developed **JSON-Based Data Access Layer (DAL)**, completely eliminating the dependency on traditional SQL servers for portability and demonstration purposes.

The project utilizes a **Progressive Architecture**, using Server-Side Rendering for core logic while enhancing the user experience with Client-Side JavaScript for real-time interactivity.

> **Note:** The user interface (UI) of this project is designed in **Turkish** to simulate a local market environment, while the codebase and documentation follow **English** standards.

## 🌟 Key Features

### 🔐 Advanced Auth & Security
*   **Whitelist Registration System:** Only approved/verified email addresses (simulated via service layer) can register.
*   **Role-Based Access Control (RBAC):** Distinct interfaces and privileges for Standard Users and Admins.
*   **Admin Dashboard:** A dedicated panel for Admins to add, delete, and update products dynamically.
*   **Password Recovery Simulation:** A system that generates a random temporary password and logs it to a server-side text file (simulating SMS/Email delivery) instead of a traditional 2FA flow.

### 🛒 Smart Shopping & Orders
*   **Personalized Cart with Key-Prefixing:** A robust shopping cart system running on `LocalStorage`. It uses a unique identifier prefix (e.g., `cart_user123`) to prevent data conflicts when multiple users access the app from the same browser.
*   **Hybrid Order Processing:** The cart is managed client-side for performance, but upon checkout, data is securely transferred and persisted server-side (JSON).
*   **Order History:** Users can view their past orders, including detailed product lists, prices, and dates.

### 🎨 Modern UI/UX & Interactivity
*   **AJAX Live Search:** Instant product suggestions with images and prices as you type, without reloading the page.
*   **Client-Side Filtering:** Instant filtering by Category and Dynamic Price Range (Slider + Input integration).
*   **Personalized Theming:** Users can select a color theme (Blue, Red, Dark Mode, etc.), which is saved and remembered via LocalStorage.
*   **Dynamic Notifications:** Integrated `SweetAlert2` for modern, animated toast notifications instead of native browser alerts.
*   **Visual Effects:** Seasonal background animations (CSS Snowfall) and dynamic slogan rotators.

---

## 🏗️ Technical Architecture

This project demonstrates a **"File-Based Persistence"** approach, reducing infrastructure costs and complexity while maintaining relational data integrity logic.

### 1. Data Layer (Custom DAL)
Instead of SQL Server, the `App_Data` folder acts as the database using structured JSON files:
*   `kullanici_listesi.json`: Stores Users, Hashed Passwords, Personal Messages, and Order History.
*   `urunler.json`: Stores Product Catalog and Stock info.
*   **Data Manager:** A custom C# class acting as a **Data Serializer** to handle Read/Write operations on JSON files in real-time, effectively mocking a database transaction system without the overhead of an ORM.

### 2. Frontend-Backend Communication
*   **AJAX:** Used extensively for Live Search, Registration, Password Reset, and Cart Checkout.
*   **Model Binding:** Frontend JSON payloads are automatically mapped to C# DTOs (Data Transfer Objects) in the Controller.

---

## 🛠️ Technologies Used

| Area | Technologies |
| :--- | :--- |
| **Backend** | C#, ASP.NET MVC 5, LINQ, Newtonsoft.Json |
| **Frontend** | HTML5, CSS3, Bootstrap 4, JavaScript (ES6), jQuery |
| **Data** | JSON File-Based Storage (Custom DAL) |
| **Libraries** | SweetAlert2, FontAwesome |
| **Tools** | Visual Studio 2022, Git & GitHub |

---

## 🚀 Installation & Setup

To run this project locally:

1.  Clone the repository:
    ```bash
    git clone https://github.com/doruk-developer/ElektronikSatisProje.git
    ```
2.  Open the project in **Visual Studio** (Double click `.sln` file).
3.  **Build** the solution (NuGet packages will restore automatically).
4.  Run the application (F5).
5.  *Important Note: The system will automatically generate the necessary JSON data files (`kullanici_listesi.json`, `urunler.json`) in the `App_Data` folder upon the first run.*

---
## 🔐 Default Login Credentials

When you run the project for the first time, the system will automatically generate a default **Admin User**. You can use these credentials to access the Admin Panel:

| Role | Username | Password |
| :--- | :--- | :--- |
| **Admin** | `Doruk` | `şifre` |

> **Note:** Since the project uses a file-based database, these credentials are created in `App_Data/kullanici_listesi.json` upon the first launch.
---

## ⚠️ Technical Notes & Disclaimers

*   **Security Note:** This project uses `LocalStorage` for cart management to demonstrate client-side state handling concepts. In a production environment, sensitive data should always be validated and stored server-side (e.g., Redis or SQL Session) to prevent manipulation.
*   **Framework Version:** This project targets **.NET Framework 4.7.2** to demonstrate proficiency with established enterprise architectures. For greenfield projects, migrating to **.NET Core / .NET 6+** is recommended.

---

## 👤 Author

**Doruk** - *Full Stack Developer*
*   GitHub: [@doruk-developer](https://github.com/doruk-developer)

---

*This project was developed for educational and portfolio purposes, demonstrating modern web development principles within the MVC architecture.*