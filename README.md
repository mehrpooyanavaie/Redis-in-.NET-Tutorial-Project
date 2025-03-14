# Redis in .NET Tutorial Project

This project is a comprehensive sample demonstrating how to integrate **Redis** into a **.NET** application. Its main purpose is to educate developers on using Redis as:
- A primary database (for storing structured data such as Geo, Hash, List, Set, and String types)
- A cache and distributed cache system (to improve performance by storing temporary data)
- A message broker (using Pub/Sub for sending and receiving messages)
- A rate limiter (using Lua scripting for request limiting)

The project consists of two parts:
1. **MyNewwRedis**: An ASP.NET Core web application that provides APIs to work with various Redis data types.
2. **RedisApp**: A console application demonstrating Redis Pub/Sub functionality.

---

## Table of Contents

- [Project Features](#project-features)
- [Project Structure](#project-structure)
- [Prerequisites](#prerequisites)
- [Installation and Setup](#installation-and-setup)
- [Detailed Explanations](#detailed-explanations)
  - [GeoLocation](#geolocation)
  - [Hash Type](#hash-type)
  - [List Type](#list-type)
  - [Set Type](#set-type)
  - [String Type](#string-type)
  - [Distributed Cache](#distributed-cache)
  - [Message Broker (Pub/Sub)](#message-broker-pubsub)
  - [Rate Limiting](#rate-limiting)
- [Conclusion](#conclusion)
- [Who Will Benefit](#who-will-benefit)
- [Contact](#contact)

---

## Project Features

- **Comprehensive Redis Tutorial in .NET:** This project provides real-world examples of using Redis with .NET.
- **Working with Various Redis Data Types:**
  - **Geo:** Store and retrieve geographical data using Redis Geo commands.
  - **Hash:** Manage structured data in key/value pairs using Redis Hashes.
  - **List:** Implement queues with Redis Lists using push/pop operations.
  - **Set:** Use Redis Sets to store unique elements.
  - **String:** Store textual data with optional expiration times.
- **Distributed Cache:** Uses `IDistributedCache` to cache data (e.g., a product list from SQL Server) to reduce database load.
- **Message Broker:** Demonstrates the Pub/Sub pattern in Redis for sending and receiving messages in both web and console applications.
- **Rate Limiting:** Implements a sliding window rate limiter using a Lua script within middleware to control request rates.

---

## Project Structure

### MyNewwRedis (ASP.NET Core Web Application)
- **Controllers:**  
  - **GeoLocationController:** Handles geographical data using Redis Geo commands.
  - **HashTypeController:** Implements CRUD operations using Redis Hashes.
  - **ListTypeController:** Demonstrates operations with Redis Lists (adding, retrieving, updating, and deleting elements).
  - **SetTypeController:** Manages unique collections using Redis Sets.
  - **StringTypeController:** Stores string data with optional expiration.
  - **ProductController:** Uses Redis as a distributed cache to store product data from SQL Server.
  - **MessageCenterController:** Publishes messages using Redis Pub/Sub.
  - **RateLimitedController:** Provides examples of rate-limited endpoints.
- **Middleware:**  
  - **SlidingWindowRateLimiterMiddleware:** Applies rate limiting using a Lua script.
  - **MessageCenterSubMiddleware:** Subscribes to Redis channels to display received messages.
- **Data:**  
  - **DbContextClass & ProductConfiguration:** Configures EF Core with SQL Server and seeds initial product data.

### RedisApp (Console Application)
- Demonstrates how to use Redis Pub/Sub by subscribing to channels and publishing messages.
- **RedisHelper:** A helper class to connect to Redis, retrieve the database, and subscribe to channels.

---

## Prerequisites

- **.NET 7.0** (or later)
- **Redis Server:** Running either locally or on a server (configurable in the settings)
- **SQL Server:** For using EF Core as the primary database for product data
- **Visual Studio 2022** or another preferred IDE for development and debugging

---

## Installation and Setup

1. **Clone the Repository:**
   ```bash
   git clone <repository-url>
   cd <repository-folder>
   ```

2. **Configuration:**
   - Update the `appsettings.json` file with your Redis connection details and SQL Server connection string.

3. **Database Setup:**
   - Run EF Core migrations to set up the SQL Server database and seed initial data.

4. **Run Redis Server:**
   - Ensure your Redis Server is running at the specified host and port (e.g., `localhost:6379`).

5. **Start the Web Application:**
   - Run the **MyNewwRedis** project. Use Swagger UI in development mode to test the APIs.

6. **Run the Console Application:**
   - Run the **RedisApp** project to see the Pub/Sub functionality in action.

---

## Detailed Explanations

### GeoLocation

- **Purpose:**  
  The GeoLocation module demonstrates how to use Redis Geo commands to store and retrieve geographical data efficiently. This section is particularly useful for applications that require geospatial queries such as finding nearby locations, calculating distances, or mapping geographical data.

- **Functionality:**  
  - **Fetching Data from IP:**  
    The `AutoAddGeoLocationFromIp` method fetches geolocation details by calling an external service (FreeGeoIP) based on the provided IP address. This service returns data such as the city name, latitude, and longitude.
    
  - **Storing Geo Data:**  
    Once the geolocation information is retrieved, it is stored in Redis using the `GeoAddAsync` command. The data is stored as a `GeoEntry` with an appended GUID in the name to ensure uniqueness.
    
  - **Retrieving Geo Data:**  
    The API provides methods to retrieve the stored geolocation data using `GeoPositionAsync`. This enables you to get precise location coordinates for any stored entry.
    
  - **Extended Capabilities:**  
    Beyond simply adding and retrieving locations, Redis Geo features allow for more advanced geospatial operations:
    - **Proximity Search:** Query for locations within a specific radius.
    - **Distance Calculation:** Compute the distance between two geo entries.
    - **Sorting by Location:** Retrieve and sort entries based on their distance from a given point.
    
    While the current implementation focuses on basic add/retrieve operations, it lays the groundwork for integrating more complex geospatial queries into your application.

### Hash Type

- **Purpose:** Manage structured data as key/value pairs using Redis Hashes.
- **Functionality:**  
  - Uses commands like `HashSetAsync` and `HashGetAllAsync` to store and manipulate `RedisModel` objects.
  - Provides API endpoints for creating, updating, retrieving, and deleting hash-based data.

### List Type

- **Purpose:** Demonstrates list operations (queues) in Redis.
- **Functionality:**  
  - Uses commands like `ListLeftPushAsync` and `ListRightPushAsync` to add elements to a list.
  - Includes methods for retrieving, updating, and deleting list elements, as well as popping items from the left or right.

### Set Type

- **Purpose:** Use Redis Sets to store unique data.
- **Functionality:**  
  - Implements operations for adding (`SetAddAsync`), retrieving (`SetMembersAsync`), and removing items from a set.
  - Useful for scenarios where duplicate data must be avoided.

### String Type

- **Purpose:** Store data as strings, with the option to set expiration times.
- **Functionality:**  
  - Uses `StringSetAsync` for saving data along with an optional expiry.
  - Provides endpoints for updating, retrieving, and deleting string data.

### Distributed Cache

- **Purpose:** Use Redis as a distributed cache to reduce load on the primary SQL Server database.
- **Functionality:**  
  - The `ProductController` leverages `IDistributedCache` to cache product data.
  - When a new product is added, the cache is cleared to ensure data consistency.

### Message Broker (Pub/Sub)

- **Purpose:** Demonstrate Redis Pub/Sub for sending and receiving messages.
- **Functionality:**  
  - The `MessageCenterController` publishes messages to a specific Redis channel.
  - Both the web middleware and the console application subscribe to channels to display incoming messages.

### Rate Limiting

- **Purpose:** Limit the number of incoming requests using a sliding window algorithm.
- **Functionality:**  
  - The `SlidingWindowRateLimiterMiddleware` intercepts requests and uses a Lua script to track the number of requests over a specified time window.
  - If the request count exceeds the limit, a `429 Too Many Requests` response is returned.
  - Rate limiting rules are defined in the configuration file.

---

## Conclusion

This project serves as a complete guide and practical reference for integrating Redis with .NET. It covers various use cases including primary data storage, caching, distributed caching, message brokering, and rate limiting. Whether you are a beginner or an experienced developer, this project offers valuable insights and code samples for leveraging Redis in your .NET applications.

---

## Who Will Benefit

This project is especially useful for:
- **.NET Developers:** Looking to integrate Redis into their applications.
- **System Architects:** Designing scalable and high-performance systems.
- **DevOps Engineers:** Implementing caching and messaging solutions.
- **Technical Enthusiasts:** Interested in learning about Redis data types and advanced features in .NET.

---
## 📜 License

This project is open-source and free to use under the MIT License.

---
## Contact

If you have any questions about Redis or its applications in .NET, please feel free to contact me via email at **[mnavaienezhad@gmail.com]**.

---

This README covers all the aspects required for professional documentation and serves as a comprehensive guide for anyone interested in using Redis with .NET.
