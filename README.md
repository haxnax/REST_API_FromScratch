# ⚡ C# REST API From Scratch

### No ASP.NET Core. No Web API framework. Just C#, HTTP, and a questionable amount of curiosity. 🧠

Ever wondered what happens behind the scenes when you call an API?

I did. So instead of just using a framework, I decided to explore how the magic actually works — by building a REST API from scratch using **C# and `HttpListener`**.

The goal? Understand the fundamentals instead of blindly trusting the magic. 🪄

---

## 🤔 What is this project?

This is a learning project inspired by the article [Building a Lightning-Fast RESTful API from Scratch in C#](https://medium.com/@bhargavkoya56/building-a-lightning-fast-restful-api-from-scratch-in-c-no-frameworks-required-888be786f5ed).

Instead of relying on ASP.NET Core to handle everything, I'm exploring how to build the essential pieces of an HTTP API myself.

Think of it as opening the hood of a car instead of just pressing the accelerator. 🚗

## 🛠️ Tech Stack

- 💻 **C#** — the brains of the operation
- 🌐 **.NET** — the runtime that keeps things moving
- 📡 **HttpListener** — listening for incoming HTTP requests
- 🔀 **Custom Routing** — sending requests to the right handler
- 📦 **JSON** — because applications need to exchange data
- ⚡ **async/await** — keeping operations from unnecessarily blocking
- 🧠 **Built-in .NET libraries** — less magic, more understanding

## ✨ What I'm Exploring

- [x] Starting a basic HTTP server
- [x] Listening for incoming requests
- [x] Understanding HTTP methods and status codes
- [x] Building a custom routing system
- [x] Working with asynchronous request handlers
- [ ] Improving request and query-string parsing
- [ ] Handling errors consistently
- [ ] Supporting JSON request and response bodies
- [ ] Exploring middleware and request pipelines
- [ ] Testing endpoints and edge cases
- [ ] Improving security and reliability

*The checklist reflects my learning roadmap — progress depends on what is implemented in the current version.*

## 🧩 How Does It Work?

Here is the basic journey of an HTTP request:

```text
      🌍 API CLIENT
           |
           | HTTP Request
           v
    ┌───────────────┐
    │  HttpListener │
    └───────┬───────┘
            |
            v
    ┌───────────────┐
    │ RequestParser │
    └───────┬───────┘
            |
            v
    ┌───────────────┐
    │    Router     │
    └───────┬───────┘
            |
            v
    ┌───────────────┐
    │ Route Handler │
    └───────┬───────┘
            |
            v
    ┌───────────────┐
    │  ApiResponse  │
    └───────┬───────┘
            |
            v
       📦 JSON / HTTP
```

The idea is simple: receive a request, understand it, find the correct handler, and send a response.

Simple in theory. Interesting in C#. 😅

## 🚀 Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- A code editor such as [Visual Studio Code](https://code.visualstudio.com/) or Visual Studio
- Curiosity and a willingness to debug things

### Run the project

Clone the repository:

```bash
git clone <YOUR-REPOSITORY-URL>
```

Navigate into the project folder:

```bash
cd <YOUR-PROJECT-FOLDER>
```

Restore dependencies:

```bash
dotnet restore
```

Run the application:

```bash
dotnet run
```

If the server is configured to listen on port `8080`, open:

```text
http://localhost:8080/
```

**Heads-up:** The server must have a matching route registered for an endpoint to return a meaningful response. Your browser is not a mind reader. Neither is your router. 🤷

## 📡 HTTP Methods I'm Learning

| Method | Purpose |
|---|---|
| `GET` | Retrieve data |
| `POST` | Create something new |
| `PUT` | Update an existing resource |
| `DELETE` | Delete a resource |
| `OPTIONS` | Check communication options, including CORS preflight |

Each request has a job. `GET` asks questions, `POST` creates things, and `DELETE` is why we need backups. 💀

## 🧠 What Am I Learning?

This project is about more than making an endpoint return `200 OK`.

I'm using it to understand:

- How HTTP requests and responses work.
- How a server receives and processes requests.
- How routing maps URLs to handlers.
- Why delegates such as `Func<ApiRequest, Task<ApiResponse>>` are useful.
- How asynchronous programming works with `async` and `await`.
- How JSON serialization turns C# objects into data clients can understand.
- How frameworks simplify tasks that would otherwise require a lot of manual code.

The real win is understanding **what the framework does for me** — not just knowing which NuGet package to install.

## ⚠️ A Small Reality Check

This is an educational project, not a production-ready replacement for ASP.NET Core.

Building an HTTP server is one thing. Making it secure, robust, scalable, and production-ready is another.

For real-world applications, established frameworks such as ASP.NET Core provide important infrastructure and security features that should not be casually reinvented.

For learning purposes, though? Rolling up your sleeves is half the fun. 🔥

## 📚 Inspiration

This project was inspired by:

[Building a Lightning-Fast RESTful API from Scratch in C# — Bhargava Koya](https://medium.com/@bhargavkoya56/building-a-lightning-fast-restful-api-from-scratch-in-c-no-frameworks-required-888be786f5ed)

The article provided the inspiration to explore the fundamentals of building an API without relying on a full web framework.

## 🎯 The Mission

Understand the fundamentals. Write the code. Break things. Debug them. Learn why they broke.

Because being a developer isn't just about knowing how to use a tool.

It's about understanding what's happening when the tool does its job.

**One HTTP request at a time.** 🚀

---

⭐ If you're also learning C# and wondering what happens behind the scenes of a REST API, feel free to explore the project!
