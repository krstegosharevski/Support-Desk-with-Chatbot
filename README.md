# Support-Desk Application with Chatbot

This project is a Support Desk application designed to streamline issue tracking and customer support. The system allows members to submit tickets, admins to assign them to handlers, and handlers to resolve them. A chatbot is integrated to assist new users in navigating the system.

## Features

### Roles
- **Admin**: Can view all tickets and assign them to handlers.
- **Handler**: Can view tickets assigned to them and provide responses to resolve issues.
- **Member**: Can create tickets (reports), including attaching images, descriptions, and adding comments. Members can also receive responses from handlers.

### Ticket System
- **Create Ticket**: Members can open a new ticket with a title, description, and optional image attachments.
- **Assign Ticket**: Admins can view open tickets and assign them to handlers.
- **Handle Ticket**: Handlers can view their assigned tickets, see the details, and respond to the member's queries or issues.
- **Commenting**: Both members and handlers can add comments to tickets for further clarification or updates.

### Chatbot
An integrated chatbot is available to help new users understand the platform and navigate its features more easily.

## Technologies Used

- **Backend**: .NET
- **Frontend**: Angular
- **Database**: Microsoft SQL Server
- **Chatbot**: Chatling

## Getting Started

### Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download) 6.0 or higher
- [Node.js](https://nodejs.org/) with Angular CLI
- [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server)


## Usage

- **Member**: Create tickets, upload images, and comment on tickets.
- **Admin**: Assign tickets to handlers.
- **Handler**: View and respond to tickets.
- **Chatbot**: Type questions to get help navigating the system.
