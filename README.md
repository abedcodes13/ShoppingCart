# E-Corp Sales Board Site

This project is a sales board web application built with **ASP.NET**. The E-Corp Sales Board Site enables users to buy and sell items within a secure and intuitive online marketplace. Sellers can list items for sale, and buyers can browse and purchase items from various users. Built with ASP.NET, this application leverages robust .NET features for smooth authentication, database management, and dynamic content rendering.

## Features

### User Authentication
- **Login Page**: Users can log in to access the site. First-time users will be directed to create a profile.
- **User Details**: Users can update their profile information, including name, age, and profile picture.

### Seller Pages
- **User Home Page**: Displays a list of all items the user has for sale. It also includes information on customers who have bought items and their total spending.
- **Add/Edit Item**: Sellers can add new items for sale and edit existing ones, including managing stock levels.

### Buyer Pages
- **All Items Report**: Displays all available items from all users with a search feature to filter results by keywords (case-insensitive and partial matches).
- **View Item**: Shows detailed information about an item, allowing users to add items to their shopping cart with stock control in place.
- **Shopping Cart**: Allows users to view and manage items in their cart before proceeding to purchase.

### Admin Capabilities
- Admin users can add, edit, and delete items from any seller on the platform.

## Project Structure

### Pages
1. **Login Page**: Manages user authentication and directs new users to set up their profile.
2. **User Details Page**: Enables users to update personal details.
3. **User Home Page**: Displays items for sale by the user and a report of customers and spending.
4. **Add/Edit Item Page**: Allows sellers to manage item details and stock levels.
5. **All Items Report Page**: Lists all items for sale with a search function.
6. **View Item Page**: Provides item details and an option to add to the shopping cart.
7. **Shopping Cart Page**: Displays selected items for purchase.

### Header
Each page (excluding login) includes a header with the current user’s details and a logout link.

## Getting Started

### Prerequisites
- An ASP.NET-compatible web server.
- Database setup for user data, items, and transactions.

### Installation
1. Clone this repository.
2. Configure the database connection in the ASP.NET settings.
3. Install necessary dependencies and run the application.

### Running the Project
1. Start the ASP.NET server.
2. Navigate to `http://localhost:[PORT]` in a web browser.
3. Log in or create a new profile to start using the application.
