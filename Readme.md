﻿
# Cafe-NET-API

	1. Intro
		1.1 This .NET 8 API project for a Cafe-Employee Management
		1.2 SQLite & .NET 8
		1.3 SQlite DB, Tables and Seed Data is Generated on Run
  		1.4 Works together with https://github.com/antaresQ/cafe-react-fe

	2. Setup
		2.1 Use VSCode or Visual Studio
		2.2 Install .NET 8 SDK and .NET 8 Runtime add ons for VS/VSCode
		2.3 Install DB Browser for accessing SQLite
		2.4 Install Docker Desktop for hosting site and containerized debugging
  		2.5 git pull https://github.com/antaresQ/cafenetapi
		2.6 in appsetting.json -> "AllowedOrigins" list, add the url and port of the deployed cafe-react-fe project 
   		2.7 To build: docker build -f ./Dockerfile -t cafenetapi .
     		2.8 To deploy: docker run -d --rm -it -p 8060:8080 --name CafeNetAPI cafenetapi
	
 	3. Use 
  		3.1 Swagger API page will launch to display API reference
		3.2 API Ref Doc: http://localhost:8060/swagger/index.html
		

	antaresQ
