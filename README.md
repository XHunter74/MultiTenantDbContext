# MultiTenantDbContext
When developing multi-tenant applications, it's common to encounter the requirement that each client has their own dedicated database. One option is to create a new DbContext with the appropriate connection string every time you access a client's database. However, this approach can quickly become cumbersome and leads to less clean and harder-to-maintain code.

Fortunately, Entity Framework provides the IDbConnectionInterceptor interface, which allows you to dynamically modify the connection string at runtime. You can retrieve the customerId from authentication claims or from a dedicated service that stores the current client context. Both DbContext and IDbConnectionInterceptor support dependency injection, making this integration straightforward and flexible.

The exact implementation will depend on the project’s requirements and the developer’s creativity.
