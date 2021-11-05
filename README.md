# Learn DDD and EF Core: Preserving Encapsulation

This are my course notes follownig along the course on [Pluralsight](https://app.pluralsight.com/library/courses/ddd-ef-core-preserving-encapsulation/table-of-contents).


---

Notes:

### Encapsulate the DbContext
- Expose as low of a configuration surface as possible
- Only allow to configure things that change depending on environment

```csharp
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
    {
        builder
            .AddFilter((category, level) =>
                category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
            .AddConsole();
    });

    optionsBuilder
        .UseSqlServer(_connectionString);

    if (_useConsoleLogger)
    {
        optionsBuilder
            .UseLoggerFactory(loggerFactory)
            .EnableSensitiveDataLogging();
    }
}
```



> Make all property setters in the domain model private by default

Relationship types:
- one to one
- one to many
- many to one
- many to many

Many to One is set by, In the method:\
 ```protected override void OnModelCreating(ModelBuilder modelBuilder)```
```csharp
 x.HasOne(p => p.FavoriteCourse).WithMany();
```
---

### Navigation properites

Instead of ID properties: 
```csharp
public long FavoriteCourseId { get; set; }
```
Use navigation properties in the domain model
```csharp
public Course FavoriteCourse { get; private set; }
```

## LazyLoading

- Install Nuget Package "Microsoft.EntityFrameworkCore.Proxies" and add to optionsBuilder in ```OnConfiguring``` method
```csharp
optionsBuilder
    .UseLazyLoadingProxies();
```

- Make domain classes non-sealed
- Protected parameter-less constructors
```csharp
protected Student()
{
}
```
- Declare all navigational properties as ```virtual```

## Find method over linq extension method 
such as FirstOrDefault() or SingleOrDefault()

DbContext linq requests ```FirstOrDefault()``` does not store retrived values in cache.  Use ```Find(12)```,\
as it caches response, more efficient. (Identity map is a cache).

Linq extension method should be used when reading multiple entities from the database.


## Equality

Entities should be responsible to checking themselves and not by equal operator on their Ids i.g. ```student1.Id=student2.Id```\
You should avoid doing this.  It is a violation of SoC.
Instead use ```student1==student2```, the entities should be responsible for the equality check.  
- This can be done by introducing a BaseEntity class, which entities can inherit from 

```csharp
public abstract class Entity
{
    public long Id { get; }

    protected Entity()
    {
    }

    protected Entity(long id)
        : this()
    {
        Id = id;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is Entity other))
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetRealType() != other.GetRealType())
            return false;

        if (Id == 0 || other.Id == 0)
            return false;

        return Id == other.Id;
    }

    public static bool operator ==(Entity a, Entity b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Entity a, Entity b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        return (GetRealType().ToString() + Id).GetHashCode();
    }

    private Type GetRealType()
    {
        Type type = GetType();

        if (type.ToString().Contains("Castle.Proxies."))
            return type.BaseType;

        return type;
    }
}
```

The ```GetRealType()``` method checks if proxies/LazyLoading and compares the basetype instead.

---


## One-to-many relationship

Normally introduced as :
```csharp
public virtual ICollection<Enrollment> Enrollments { get; set; }
```

Add row added using:
```csharp
student.Enrollments.Add(
    new Enrollment(course, student, grade));
```

However, this approach is does break encapsulation.  As it allows a to wide API surface area.

#### Encapsulate the enrollment collection
Correct way to do it:\
Use a backing field for the setter and public property for getter in the ```Student.cs```. 
Make the collection readonly.
```csharp
private readonly List<Enrollment> enrollments = new List<Enrollment>();
public virtual IReadOnlyList<Enrollment> Enrollments
{
    get
    {
        return enrollments.ToList();
    }
}
```
In order to add enrollment, we need to provide a method like this in ```Student.cs```
```csharp
public void EnrollIn(Course course, Grade grade)
{
    var enroll = new Enrollment(course, this, grade);
    enrollments.Add(enroll);
}
```
Add a row added using:
```csharp
student.EnrollIn(course, grade);
```
***For all one-to-many relationship.***  
>The one on the self should be responsible for creation and deletion of it collection

There is no need to add Enrollment to the DbSet<> in the Student Context as it's an internal entitie.  
It's controlled by the aggregate route ```Student.cs```

### Egar load collections

Collection classes in backing field are not Lazy loaded by EFCore.  
There are multiple ways to cause EFCore to load it. Here is an example 
using [eager](https://www.c-sharpcorner.com/UploadFile/abhikumarvatsa/what-is-eager-loading-and-what-is-lazy-loading-and-what-is-n/) loading:

```csharp
Student student = context.Students
        .Include(x => x.Enrollments)
        .Single(x => x.Id == studentId);   
```

However, this method does not cache the response.  An alternative is to make to calls to the database:
```csharp
Student student = context.Students.Find(studentId);
context.Entry(student).Collection(x => x.Enrollments).Load();
```




































