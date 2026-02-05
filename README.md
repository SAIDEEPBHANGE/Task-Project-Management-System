# 🗂️ Blazor Task & Project Management System

A full-stack task and project management system built with **Blazor** and **ASP.NET Core**.

Think **Trello + Jira (lite)** — designed to demonstrate real-world Blazor patterns, clean architecture, authentication, state management, and scalable UI practices.

> 🚀 Built as a learning-first but production-style project to cover ~90% of what matters in Blazor development.

## ✨ Features

### 🔐 Authentication & Authorization
- User registration & login
- Role-based access (Admin, Member)
- Secure pages using `[Authorize]`
- Role-based UI rendering

### 📁 Project Management
- Create, edit, delete projects
- Assign members to projects
- Project-level permissions

### ✅ Task Management
- Create, edit, delete tasks
- Task status workflow:
  - To Do
  - In Progress
  - Done
- Assign users to tasks
- Due dates & descriptions

### 📊 Dashboard
- Project overview
- Task analytics (by status)
- Charts & summaries

### 🔄 Real-Time Updates (Optional / Advanced)
- Live task updates using SignalR
- Multi-user collaboration

### 🎨 UI & UX
- Responsive design
- Modal dialogs
- Toast notifications
- Built with **MudBlazor** (recommended)

---

## 🧠 Blazor Concepts Covered

### 1️⃣ Blazor Fundamentals
- Components & Razor syntax
- Parameters & `EventCallback`
- Component lifecycle methods
- Two-way binding
- Forms & validation

**Example Components**
- `TaskForm`
- `ProjectCard`
- `TaskList`

---

### 2️⃣ Routing & Layouts
- `@page` routing
- Route parameters
- Layouts & nested layouts
- `NavigationManager`

**Example Routes**
```

/projects
/projects/{id}/tasks

```

---

### 3️⃣ State Management
- Cascading parameters
- Scoped services
- App state container
- Local storage usage

**Example**
- Logged-in user state
- Selected project state

---

### 4️⃣ Authentication & Authorization
- ASP.NET Core Identity
- JWT (for WebAssembly)
- `[Authorize]` attributes
- Role-based UI logic

**Example**
> Only Admin users can delete projects

---

### 5️⃣ Data Access
- Entity Framework Core
- Code-first migrations
- Repository pattern
- DTOs for API boundaries

**Entity Relationships**
```

User → Projects → Tasks

```

---

### 6️⃣ Blazor Hosting Models

You can run this project in two ways:

#### ✅ Blazor Server (Recommended First)
- Easier setup
- Real-time by default
- Faster learning curve

#### 🌐 Blazor WebAssembly (Advanced)
- API-based architecture
- Frontend / backend separation
- JWT authentication

> 👉 Suggested path:  
> **Start with Blazor Server → then migrate to WASM + API**

---

### 7️⃣ API & HTTP
- RESTful APIs
- `HttpClient`
- Error handling
- Loading states & UX feedback

**Example**
```

GET /api/tasks
POST /api/projects

```

---

### 8️⃣ UI Libraries
- MudBlazor (recommended)
- Bootstrap / Radzen (optional)
- Dialogs, snackbars, responsive layouts

---

### 9️⃣ Advanced Features (Optional 🔥)
- SignalR real-time updates
- Charts & analytics
- File uploads (task attachments)
- Background jobs

---

### 🔟 Testing & Performance
- Component testing with **bUnit**
- Dependency Injection
- Lazy loading
- Error boundaries

---

## 🗂️ Project Structure

```

/Client
/Components
/Pages
/Services

/Server
/Controllers
/Data
/Models

/Shared
/DTOs

````

---

## 🧭 Learning Roadmap

### Phase 1 – Core CRUD & UI
- Projects & tasks
- Component design
- Forms & validation

### Phase 2 – Authentication & Roles
- Identity setup
- Authorization rules
- Role-based UI

### Phase 3 – State & Services
- App state container
- Cascading values
- Scoped services

### Phase 4 – API & WASM
- REST APIs
- DTOs
- Blazor WebAssembly

### Phase 5 – Real-Time & Polish
- SignalR
- Charts
- Performance tuning

---

## 💡 Why This Project?

✅ Real-world architecture  
✅ Scales from beginner → advanced  
✅ Interview-ready  
✅ Portfolio-worthy  
✅ Teaches **why**, not just **how**

---

## 🛠️ Tech Stack

- **Blazor Server / WebAssembly**
- **ASP.NET Core**
- **Entity Framework Core**
- **ASP.NET Identity**
- **MudBlazor**
- **SignalR**
- **SQL Server / SQLite**

---

## 🚀 Getting Started

1. Clone the repository
2. Update the connection string
3. Run database migrations
4. Start the application

```bash
dotnet ef database update
dotnet run
````

---

## 📌 Future Improvements

* Notifications system
* Task comments
* Audit logs
* Dark mode

---

## 📄 License

MIT

---

## 🙌 Author

Built as a learning-focused Blazor project to master real-world full-stack development.
---

If you want, next I can:
- Customize this for **Blazor Server only** or **WASM only**
- Rename it for **personal branding**
- Add **screenshots / badges**
- Write a **resume-ready project description**
- Generate the **initial folder + boilerplate code**

Just say the word 👇