# LiveCrawlingDemo - Crawl traditional Vietnamese lottery result
Techstack: Dot Net 7, SQL Server, Entity Framework Core, jQuery.

Create database using Code-first migration.

Using Entity Framework Core to create and update data in database.

Using Hangfire library to executes recurring jobs as create and update live lottery result.

Using Fizzler HtmlAgilityPack to crawling data from external sources, e.g ketqua.net website (https://ketqua9.net/).

Using SignalR to update real-time lottery result to client view.

Create database in SQL Server:
In Visual Studio, select Tools -> Nuget Package Manager -> Package Manager Console.
Enter: update-database then run project to generate database, in SQL Server you choose Authentication type is Windows Authentication.
