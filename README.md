# CybersecurityBotGUI - POE

A Cybersecurity Awareness Chatbot built with C# WPF for South African citizens.

## Features

### Part 1 - Console Chatbot
- Voice greeting on startup
- ASCII art logo
- Basic cybersecurity responses
- Input validation

### Part 2 - GUI Chatbot
- WPF graphical interface
- Keyword recognition
- Sentiment detection
- Memory and recall
- Random responses
- Conversation flow

### Part 3 - Advanced Features
- Task Manager with MySQL database (add, complete, delete tasks with reminders)
- Cybersecurity Quiz (12 questions with scoring and feedback)
- NLP simulation (recognises different ways of phrasing requests)
- Activity Log (tracks all actions with timestamps)

## How to Run
1. Clone the repository
2. Open `CybersecurityBotGUI_part_2.sln` in Visual Studio
3. Install MySql.Data NuGet package
4. Set up MySQL database using the provided SQL script
5. Build and run the project

## Database Setup
```sql
CREATE DATABASE IF NOT EXISTS CyberBotDB;
USE CyberBotDB;
CREATE TABLE IF NOT EXISTS Tasks (
    TaskId INT AUTO_INCREMENT PRIMARY KEY,
    Title VARCHAR(255) NOT NULL,
    Description VARCHAR(500),
    ReminderDate DATETIME NULL,
    IsCompleted BOOLEAN NOT NULL DEFAULT FALSE,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);
```

## Technologies Used
- C# WPF (.NET Framework)
- MySQL Database
- MySql.Data NuGet Package
- GitHub Actions CI/CD
