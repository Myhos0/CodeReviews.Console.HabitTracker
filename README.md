# HabitTracker
My third console project at C# Academy.  
A console application where we have daily habit tracker.  
# Requirements:  
- [x] This is an application where you’ll log occurrences of a habit.
- [x] This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day) Users need to be able to input the date of the occurrence of the habit
- [x] The application should store and retrieve data from a real database
- [x] When the application starts, it should create a sqlite database, if one isn’t present.
- [x] It should also create a table in the database, where the habit will be logged.
- [x] The users should be able to insert, delete, update and view their logged habit.
- [x] You should handle all possible errors so that the application never crashes.
- [x] You can only interact with the database using ADO.NET. You can’t use mappers such as Entity Framework or Dapper.
- [x] Follow the DRY Principle, and avoid code repetition.
- [x] Your project needs to contain a Read Me file where you'll explain how your app works and tell a little bit about your thought progress. What was hard? What was easy? What have you learned? Here's a nice example:
# Features:
- SQLite database connection
    - The program uses SQLite to create a database to store and read information.
    - When the program is started, the application generates the database and tables necessary for its operation.
    - When the application is launched, the program creates a new habit and inserts 100 values for testing over a period of three years (2023-2026).
- The application uses a Spectre Console UI where we can navigate using the keyboard.
    - <img width="298" height="173" alt="Captura de pantalla 2026-01-31 152626" src="https://github.com/user-attachments/assets/d61ccccd-47ad-41b5-931f-7d0fc206eb69" />
    - The first window shows us a menu where we can choose whether to create or view a habit.
    - The menu for creating a habit asks for the habit's name and its type.
    - <img width="644" height="175" alt="Captura de pantalla 2026-01-31 154437" src="https://github.com/user-attachments/assets/7e09a86e-b834-4ec4-aa7c-e00ee821ed49" />
    - And in the query menu, we select the ID of the habit we want to query.
    - <img width="1480" height="233" alt="Captura de pantalla 2026-01-31 155626" src="https://github.com/user-attachments/assets/f076be72-9b89-42d1-b5f3-ab7d9abbee71" />
- CRUD DB functions
    - The application has a menu with basic operations such as create, read, update, and delete.
    - <img width="289" height="245" alt="Captura de pantalla 2026-01-31 160449" src="https://github.com/user-attachments/assets/3a110b60-8a31-4687-8993-6d3de5dfda94" />
    - It also has a menu with more queries, such as the total, the average per month, and per year.
    - <img width="277" height="203" alt="imagen" src="https://github.com/user-attachments/assets/c8898b84-822d-4a14-b1e7-2e533d32e0ec" />
# Challenges
- One of the problems I encountered when developing this project was creating habits with two different types of data, namely integers and decimals.
- Adding new queries such as averages was also complex due to the type of date stored in the data phase.
- Adding the feature that allows users to create new habits was also difficult, as I had to refactor how the program worked when it was first launched.
# What I have learned
- Use spectre console to view data with tables.
- Use parameterized queries for query security, although I'm not sure if I've implemented it correctly.
- Use different classes to make the code more readable and try not to repeat code.
- Use SQlite to utilize a local database and learn more functions to implement in the program.
- Create a README file explaining how the application works.
# Areas To Improve
- I would like to learn more about the tools provided by Visual Studio.
- I would like to improve the organization of classes into different folders.
- Gain a better understanding of parameterized queries and how to apply them correctly.
- Improve how to structure a README correctly
# Resource Used
- The C# Academy video of the project: https://www.youtube.com/watch?v=d1JIJdDVFjs
- Page to learn about parameterized queries: https://reintech.io/blog/mastering-parameterized-queries-ado-net
- Guide to creating a README: https://github.com/thags/ConsoleTimeLogger
- Microsoft documentation for dates: https://learn.microsoft.com/es-es/dotnet/api/system.datetime?view=net-8.0
- Microsoft documentation for SQLite: https://learn.microsoft.com/en-us/dotnet/api/microsoft.data.sqlite.sqlitecommand?view=msdata-sqlite-9.0.0
- Microsoft documentation for SqlCommnad: https://learn.microsoft.com/es-es/dotnet/api/system.data.sqlclient.sqlcommand?view=netframework-4.8.1
