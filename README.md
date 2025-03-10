# Project Name: ABC Retailers CLDV Web Application 🎗️

## Overview: 📃
This website allows a user/admin to add products, users, clarify orders and finally upload files.
This website showcases the various azure file storage options and how they interact with various types of information.
I will explain the different storage types and the type of information the system will be handling in order to store the necessary data.

## Technology Used: 💻
For this web application I made use of .Net MVC utilizing c#, html and css code.
The use of MVC allows me to integrate complex code unlike in ASP.Net which did not offer much substance in the ways I could interact with a webpage.
The main use of C# in this web application is to both, display, upload, download and delete data stored in the various storage options in azure.

## Challenges Encountered 🛠️
I faced several challenges throughout the development of the application, one of the most noticible challenges was trying to display a MP4 video on my main home page. 
After extensive research I could just formulate and comprehend how to configure the code to get the result shown on my home page.
It was challenging tring to make the video non interactable aswell as making the video play automatically and run through a loop.

Another challenge that I faced was displaying my log of documents to a seperate View. This was difficult as I had top make changes to my files controller and allow collected 
data to display seperately.

For part 3 one of my greatest challenges was figuring out how to transfer data between MVC and Functions. I had to make use of Java Script and think outside the box.

## Lecturer Feedback: ✈️
### Part 2 Feedback:
I recieved feedback in part 2 for my functions not being able to handle user input, for this part I dedicated attention to allowing MVC content to be passed
through as form data so the function can read the data and insert it into the SQL database.

### Part 1 Feedback:
I implemented functionality to my Orders View that allows orders made by users to be updated, their status is automatically pending and they can be rejected and accepted.
This was asked originally in part 1 of the POE and I struggled to implement it, however after working through more complex html and C# applications I managed to accomplish the feature!

## To download the project: 📥
1) Select the code button on the home page.
2) Select the option 'Download ZIP' in the bottom left corner.
(The file will then be downloaded onto your device.)
3) Next you will want to launch 'Visual Studio Code 2022'.
4) Select the option to load a project and select the ZIP file dowmloaded from GIT.
5) Next the project should load and run!

## How to use the project: 🌐 

### 1. Products 🍉
As you open the web application you will be greated with navigation tabs at the top of your screen, we will start with the first option, products.

The products page will firstly allow you (the admin) to see all products available to the customers. 
You may delete these products or simply add a new one.

Just below the page heading will be an 'Add a product' link and this will allow the admin to add a new product. Simply fill in the points and you will be able to see your
newly created product on the homepage.

### 2. Users 🥑
Next we will be looking at the Users tab.

This page will display an index off all the users currently saved in the azure table database.
Just like the products tab, underneath the heading there will be an option to add another user.
Simply fill in the credentials and the user will be displayed back on the index page.

### 3. Orders 🥝
Thirdly I present the Order Processing page.

This page is dedicated to showing which users have purchased what products at what time and where.
This information is useful as it allows the admin to view what specific products have been purchased and allows the order to be tracked to a specific user. 
Underneath the Pocess heading will be a link to a new tab where the admin will be able to create a new order.

### 4. Files 🍓
Lastly there is a files tab which displays a box that holds a buttton where an admin can upload a document on regards to Vendor applications.

Underneath the upload box will be a button that takes the user to a seperate page that displays a list of all logs uploaded and allows the admin to delete the files.

# Presentation Slides

[View Presentation Slides](https://github.com/JoshuaSutherland43/ABC_Retailers/blob/main/ST10255930_PROG_POE_6212_Slide.pdf)



With all this considered I hope you enjoy my website! 🍎

