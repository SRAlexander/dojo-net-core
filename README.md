# dojo-net-core

Welcome to the .Net Core API Dojo

***Pre Requirements***

For this Dojo you will need... 

* GIT -  https://git-scm.com/downloads
* SQL Server - https://www.microsoft.com/en-gb/sql-server/sql-server-downloads
* SSMS - https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15
* Visual Studio (Not code) - https://visualstudio.microsoft.com/

***1 - Setting up a project***

Once all of the above has been installed, open up Visual Studio (2019)

**Create a basic project** 

* Select -> "Create a new project"
* Filter by C# and select "ASP.NET Core Empty"
* Provide a Project Name, Location and Solution Name. 
* Click Next
* Target Framework = .Net Core 3.1
* Authentication Type = None
* I would also check the docker option.
* Click Create

You should now see the main development console of visual studio open, by default the right-hand side should be open with the Solution Explorer tab selected. Let's look at what's been created...

Firstly, you should see at the top of the file is the "Solution" a solution can have multiple Projects which you should see directly under it.

* Dependencies > As our project grows, we can see what additional components we have installed against each project
* Properties > launchSettings.json > This is configuration provider for hosting profiles on a local development machine e.g. providing hosting URL's and ports.
* appsettings.json > Global configuration settings are set here using a json format. 
* Program.cs > Points to the Startup.cs to determine how to run our application 
* Startup.cs > This file controls all our component configurations, by default we use three...
    * Developer Exception Page > If we are running the application in debug mode, and errors are detected, we are directed to the developer exception page providing more details.
	* Use Routing > this is how we control API endpoints, by default we don't need to do much with this
	* Use Endpoints > What are our endpoints? The default configuration is to display "Hello World" when we hit the API root URL.
	
So now we have a quick understanding of the file structure, let's run the application, in the top bar click the green run triangle in IIS express mode and accept the default certification questions. 
A Browser window should now open to https://localhost:port, the port configuration coming from iisSettings configuration in launchSettings.json.

Congratulations, you have your first very basic .Net Core API, this can be found in the folder "1 - Core Dojo - New Project"

***2 - Controllers***

**Our first Controller**

Controllers are what we use to manage access to our application and who can access the endpoints they provide.
Let's create a folder for all controllers simply called controllers.
Then create a new class called Students Controller, for this first instance I will provide the code for you...

	namespace Core_Dojo.Controllers
	{
		[ApiController]
		[Route("api/[controller]")]
		public class StudentsController : Controller
		{

			//Constructor
			public StudentsController() { }

			[HttpGet("")]
			public async Task<IActionResult> GetStudent()
			{
				return Ok("This is a student");
			}

		}
	}

Things to note:-
	
	* [ApiController] - is an attribute, it indicates that our class will act as an API
	* [Route("")] - is another attribute that allows us to set our URL access point to the controller, all functions after are extensions of this route. the square brackets insert the name of the controller without the Controller section of the string
	* [HttpGet] - another constructor that indicates what http verb to use to access the route, more will be described later.
	* [async] Task<> - This is an accessor that allows us to take advantage of  asynchronous running (fire and forget), it's good practise to use this on all endpoints with an IActionResult to provide dynamic responses. Slightly Breaking .NETs strong typing rules but practical in the long run.

**Configure our app**
	
Let's configure our app to handle all our controllers by default, open the "startup.cs" file and inside the ConfigureServices function add...

	services.AddControllers(config => {});

This tells our app to look for all [APIControllers] add them to our service configuration. If we then want to use the routes declared in the controllers attributes, we can then modify the app.UseEndpoints configuration to the following...

	app.UseEndpoints(endpoints =>
	{
		endpoints.MapControllers();
	});

With that final change let's run our app and see what happens...

Damn, it's failed, or has it? By default, we hit the home URL but we configured our controllers endpoint to exist at /api/students, try giving that a go. 

Bingo, our controller implementation  is now good to go. There reason why we've added the /api/ to our endpoints is that it is possible to host a UI on the root instance and our API together, if we chose to do that, we do not want conflicted endpoints e.g. webpages /students and API function /students

**Who's got Swagger?**

So as shown above, as our application grows we don't want to have to keep track of 101 endpoints. 
There are 3rd party tools out there that can manage our endpoints for us like Postman but we can manage them ourselves via SWAGGER.

To install Swagger, we're going to use the Nuget package manager...

* Top menu > Tools > NuGet Package Manager > Manage NuGet packages for solution. 
* Now let's find swagger or as it's now packaged as Swashbuckle.AspNetCore. 
* Use the browse option to search for swagger and click on Swashbuckle.AspNetCore. Just to take note, have a look at what dependencies it requires, and you can see that it relies on a few other packages in the left hand search results. 
* Install the package against the project. 

Once complete we can see the package in under dependencies > Packages.

Now that swagger is installed, we can add it into our Startup.cs following the instructions from https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-5.0&tabs=visual-studio
Add the following to the Configure Services function...

	services.AddSwaggerGen();

And the following within the env.IsDevelopment() check...

	// Enable middleware to serve generated Swagger as a JSON endpoint.
	app.UseSwagger();

	// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.)
	app.UseSwaggerUI(c =>
	{
		c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
		c.RoutePrefix = string.Empty;
	});

By doing so, swagger will only be enabled on our development instances, not our live instances meaning we're not public displaying our whole application. 
Run our application again and if all has been configured correctly, we should see a nice Swagger UI with our student's endpoint displayed. 
The code up to this point can be found in "2 - Core Dojo - Controllers & Swagger"

***3 - A Lesson on VERBS***

We have now created our first controller instance that used the GET verb. There a few VERBS that you can use and should now about...

GET - Get is the simplest endpoint to implement, it is defined as an action to get a single static resource, the key word being Static! Therefore, GET's usually take advantage of the fact you can pass variables to them via a URL such as an ID so it gets a single, non-changing resource. If you find you are using a get to receive multiple values that can change over time, e.g. the top 10 items of a list. You should consider changing your verb to a POST.

POST – There is a common misconception that a post command is for saving data. It can indeed be used for saving data, but it should really be used for searches, filtering, and actions like pagination. The reason for this is that a URL has a limit of 2048 characters so for big queries, a GET cannot handle your request. Therefore, POST is the first of our VERBs that take advantage of the body of a request.  To simplify this knowledge, you can consider every request containing 3 items…

	* URL – link to your endpoint
 	* Headers – Information specific to the endpoint functions, e.g. authentication tokens & datatypes.
	* Body – JSON or string format data specific to the request. 

Therefore, we can pass a JSON body of data to our endpoints are very quickly turn them into objects using deserialization. 

PUT – Similar to POST but should ultimately be used for creating records or updating existing records.

DELETE – Best practises would be to use the URL with an ID of a static resource but DELETE does have access to the BODY.  

Notes:- 
	*.Net uses Parameter binding in the form of ([FromBody] Model query), to access the body data and has other bindings depending on how data is passed in. These types can be found at https://docs.microsoft.com/en-us/aspnet/web-api/overview/formats-and-model-binding/parameter-binding-in-aspnet-web-api
	*If you require an ID to be passed in through the url, you must declare it in the Route, against each action, the string inside the HTTPVerb attribute is the route e.g. [HTTPGET("/{id}")]
	*Due to the nature of VERBs, each endpoint can have a multiple routes e.g. api/students can have a GET,POST,PUT and DELETE, therefore, the endpoint name is pluralised. Multiple results use students, single uses students/{id} to indicate a singular under clean coding practises. 
	
**Task**: Using this new knowledge of VERBs, attempt to create one of each endpoint in our StudentsController. If they have been created successfully, they will show up in the swagger UI and can be tested directly from there. You can use breakpoints on your code to see what’s being passed into your function by clicking next to a line on the left-hand side of your code. 

You will also need to create the following model to test the [FromBody] attribute, I would recommend placing it inside a folder called RequestModels

	using System;

	namespace Core_Dojo.RequestModels
	{
		public class StudentAddRequest
		{
			public string FirstName {get;set;}
			public string MiddleNames { get; set; }
			public string Surname{ get; set; }
			public DateTime DateOfBirth { get; set; }
		}
	}
	
An example of functioning code can be found in "3 - Core Dojo - Verbs"

***4 - Services and Dependency Injection***

**Create a service**

One of .NETs greatest strengths is Dependency Injection by default. It allows us to easily swap out different units of code if they implement the same core functions using Interfaces. This allows us to test each section of code independently through Mocking, something we will visit later and separate out our different areas of code. Controllers as mentioned before are designed to manage incoming requests and pass back the response. Services are for our business logic!

Let’s start by creating a folder called services and adding two files, one StudentsService class and one IStudentService interface. For the time being these files can implement the same functions as in our controller since they are simple endpoints, here is an instance of the interface we want…

	using Core_Dojo.RequestModels;
	using Core_Dojo.ResponseModels;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	namespace Core_Dojo.Services
	{
		public interface IStudentsService
		{
			public Task<StudentResponse> GetStudent(int id);
			public Task<List<StudentResponse>> GetStudents();
			public Task<Boolean> AddStudent(StudentAddRequest request);
			public Task<Boolean> DeleteStudent(int id);
		}
	}

And an example student response model which should be created in a folder called ResponseModels…

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	namespace Core_Dojo.ResponseModels
	{
		public class StudentResponse
		{
			public int Id { get; set; }
			public string FirstName { get; set; }
			public string MiddleNames { get; set; }
			public string Surname { get; set; }
			public DateTime DateOfBirth {get;set;}
		}
	}

If we now have a look at our StudentsService class, we can now mark it as an instance of IStudentsService using inheritance syntax…

	public class StudentsService : IStudentsService
	{
	}

If you now hover over the IStudentsService in the class which is now underlined, Intellisense should now complain about non implemented members. Fortunately, we have Intellisense to help out here. Click on the lightbulb that pops up with the errors and select implement interface. All functions that are declared in the interface should now have a placeholder added to the class. They will however throw a Not implemented exception for the time being. Time to have a look at dependency injection. 

**Prepare our application**

Over to our Startup.cs file again, since over time our services will grow, we will create a function specifically to manage the set up of our injectable services. Create a private function call SetUpDIServices like below…

	private void SetUpDIServices(IServiceCollection services)
	{
		services.AddScoped<IStudentsService, StudentsService>();
	} 
	
And the call it from the Configure Service function.
 
	SetUpDIServices(services);

**INJECT**

We can now use the service anywhere we want, as long as we don’t create a circular dependency e.g., a parent requires a child of a parent.

All we have to do in pass the service into a constructor of a class and assign it to a readonly variable. In our instance we want to create it on the StudentsController so our controller can access the future business logic of the service…

	private readonly IStudentsService _studentsService;

	//Constructor
	public StudentsController(IStudentsService studentsService) {
		_studentsService = studentsService;
	} 

Now try assigning the relevant service calls to the current endpoints. If you succeed then your swagger responses should return a NotImplemented error at this stage.

Note:- 
* All Asynchronous functions require the await keyword at the start of the statement.
* All available functions should be shown after typing the . after _studentsService.

In the next section we will go one step deeper and complete our functions with DB access, for now the current code can be found in folder “4 - Core Dojo – Services”.

***5 - Repositories & Entity Framework***

**Starting Entity Framework**

Repositories are where we keep the logic for interacting with the database and the tool we're going to use to allow us to do that is Entity Framework.

We want to keep the repositories separate from the Core Dojo web project and so we will need to create a new project. This is to keep our solution tidy ([Separation of Concerns](https://en.wikipedia.org/wiki/Separation_of_concerns))

Create a new project by right clicking on the Solution in the Solution explorer but this time make it a Class library and name it "Repositories" and use the same Target Framework as before - ".NET Core 3.1".

Next, we want to install the Entity Framework packages through the NuGet package manager, open the tool as we did in the previous section and search for Entity. In the results we specifically want the packages “Microsoft.EntityFrameworkCore”, “Microsoft.EntityFrameworkCore.Relational”, “Microsoft.Extensions.Configuration.Json”. Click on them to install them but take care to only install them against our new repositories project.
Inside your project, you're going to need to store your Entity models. An entity is a model for an object which is stored in a table in your database. So what we'd like to store initially is our students.

Create a new folder for your entities called "Entities" within the new Repositories project.  
Inside this folder, create a new class called "Student" with the following...
 
	public class Student
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string MiddleNames { get; set; }
		public string Surname { get; set; }
		public DateTime DateOfBirth { get; set; }
		public DateTime CreatedTimestamp { get; set; }
	}
	
We also want a single “Context” to manage what we want add to our database, we need to create a model the model above and add it as a reference in our context file. So lets create this context outside of the Entities folder. I’ve called mine RepositoryContext.cs…

	namespace Repositories
	{
		public partial class RepositoryContext : DbContext
		{
			public RepositoryContext() { }

			public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options) { }

			// List of Tables
			public virtual DbSet<Student> Students { get; set; }



			protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
			{
				if(!optionsBuilder.IsConfigured)
				{
					var webProjectLocation = System.IO.Directory.GetParent(Environment.CurrentDirectory) + "\\Core Dojo";
					IConfigurationRoot configuration = new ConfigurationBuilder()
						.SetBasePath(webProjectLocation)
						.AddJsonFile("appsettings.json")
						.Build();

					optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
				}
			}

			protected override void OnModelCreating(ModelBuilder modelBuilder)
			{
				new StudentConfigurations().Configure(modelBuilder.Entity<Student>());
			}
		}
	}

The key things to note from this file are firstly that we are now working in a new namespace since this file belongs to the “Repositories” project. We have also added a virtual DBSet called Students. The pluralisation is important for Clean Coding standards. Our model is singular, but our table definition is pluralised since a DB query will bring back multiple Student models. Lastly since we are in a class library project, access to the shared appsettings.json file can be tricky, therefore we are overriding some of the default functionality to load up the appsettings.json file directly into our DB context.   

**Connection Strings**

We are now going to take advantage of our appsettings.json file in our Core project, open it up and add the following parameter…

	"ConnectionStrings": {
    		"DefaultConnection": "Server=insert your server;Database=UniversityDatabase;Trusted_Connection=True;MultipleActiveResultSets=true;Integrated Security=True"
	},

Your server name can be found by opening your instance of SSMS, your server name will be displayed on access.
Integrated Security means that we will be using our Windows login credentials to access the database. 

**Using connection Strings**

We can now import the connection string into our application through our Startup.cs file, open it up and we will now Inject the IConfiguration interface. IConfiguration is a standard service provided by .Net Core so we don’t need to any further configuration other than the injection which requires a constructor…

	public IConfiguration _configuration { get; private set; }

	public Startup(IConfiguration configuration)
	{
		_configuration = configuration;
	}
		
We can now add the following to the ConfigureServices function to setup our DBContext with the right connection string…

	services.AddDbContext<RepositoryContext>(options =>
		options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));

**Entity Magic Migrations Time** 

For migrations we are going to need the Package Manager Console, this can be found in Tools > NuGet Package Manager > Package Manager Console
Run the following...

	dotnet tool install --global dotnet-ef

If you get a 407, follow the instructions to configure the proxy against the CLI using the instructions at… 
	http://marcomengoli.github.io/miscellaneous/NuGet-proxy-config/

With the tool installed, lets create our first migration, run the following in the CLI  from the .repositories project folder. Take note of the current timestamp to organise our migration files too…

	dotnet ef migrations add yyyymmddhhmm_Initial_Migration

If everything has been configured correctly, a migrations folder should have been created with two files, looks in our timestamped file. You will find a set of instructions to create a database table produced from our class declaration. Woop!

**Database Updates**

Okay so now we have the script, lets run it. If a database does not exist on the server, it will be created...

	dotnet ef database update
	
Once completed open up your instance of of SQL Server Management Studio and connect. You should be able to see your newly created database. Success!

**Extension**

Try adding a new column to our students table and running through the process again…

*Modify the Student Class
*Run the Migration
*Update the Database

**Repositories**

So how do we write our code to access the database easily? Well just like we created our services, we can create repositories as a starting point and inject them into our services (not controllers!). 

*Step 1: Create a folder in our repositories project called Repositories 
*Step 2: Like our services, create a class and an interface, StudentRepository and IStudentRepository
*Step 3: Follow the service examples and create similar function endpoints that will manage our database code. 
*Step 4: In our StartUp.cs file, create a new function called SetUpDIRepositories and configure our DI interface injection. Make sure you call the function in ConfigureServices
*Step 5: Inject our new repository into the StudentService using our student controller as an example
*Step 6: Call the repository functions from our service functions.


**Uh Oh – Circular Dependencies**

It looks like we cannot access our response and request models on the repository level. One project can depend on another but they both cannot depend on each other otherwise we don’t have a starting build point! So how do we address this? Simple, lets create another project. Usually, most projects have a Common project that all other projects depend on, so lets create one. 

*Right click on the solution and add another class library project called Common. 
*Delete the initial Class1.cs and move all our models into the Common project.
*Update the namespace on the models to use Common.Folder name rather than Core_Dojo.Folder.

The next step is a little tedious but every reference to the models now needs to be updated. This is a form of refactoring. Fortunately, we don’t have to many files, so let’s go through them, hover over any errors and select the “Add Common” from the Intellisense lightbulb.
Once you’ve corrected all the errors, try building your projects, if there are any more errors, they should be displayed at the bottom of the screen, double clicking them will take you straight to the error in code. 
On professional projects, you will most likely see the services separated out into their own project too, however for the purpose of this dojo, I’m not going to be that mean and make you do a second refactor. 
Make sure your Swagger API can still be reached, and that a Not Implemented error is returned before you continue onto the next section. The Code for this section can be found in "5-Repositories & Entity"


***6 - LINQ***

LINQ stands for Language Integrated Query. It ultimately it lets us build up queries against a range of data objects in .Net including lists, arrays and in what we’re about to do Database Tables. Some simple commands include…

*.Where(item => {})
*.First()
*.FirstOrDefault(item => {})
*.ToList()
*.OrderBy()
*.Group()

Some of these functions also have Asynchronous implementations which makes our life very easy to query asynchronously. Against a DB instance we also have a special class called an IQueryable. Until we perform a .first() or a .toList() operation, we can build up our queries over multiple lines without running the DB query e.g. conditional logic queries.  Operations such as add and delete are not committed to the database until a .SaveChanges action is performed against the context.
Now as a first step, we are going to add our DBContext to our StudentsRepository, since we have already setup our DB instance in our setup.cs we don’t need to configure an interface for it…


	private readonly RepositoryContext _context;

    public StudentsRepository(RepositoryContext context)
    {
 	   _context = context;
    } 
	   
We can now access our Database using _context.tableName, in our case, _context.Students.
Try writing some queries to perform our GetStudent, GetStudents, DeleteStudent and AddStudent. 
Notes:
*Our input models and output models are different, so you will need to convert the data into the new models.
*Once you’re happy with your code, run your application and try adding and retrieving Students from the database. The completed example can be found in “6 – Core Dojo – LINQ”

***7 - Filtering with Inheritance***

Some models we cannot reused, we must create them for a specific purpose. Others on the other hand, if designed correctly can be shared by multiple other models through inheritance. For this section we’re going to add a filter model to our GetStudents endpoint so we can search on large amounts of data. One aspect of filtering that should be consistent is pagination and sorting! Therefore, lets add a new folder to common called queries and add two classes, PaginationQuery and StudentFilterQuerynd their respective interfaces. Now I’m only going to give you the interfaces for this section, build the classes from the interfaces…
	
	namespace Common.Queries
	{
		public interface IPaginationQuery
		{
			int PageNumber { get; set; }
			int DisplayedPerPage { get; set; }
			string SortBy { get; set; }
			bool SortByDecending { get; set; }
			int TotalRecords { get; set; }
			int NumberOfPages { get; set; }
		}
	}
	
	namespace Common.Queries
	{
		public interface IStudentFilterQuery : IPaginationQuery
		{
			string Surname { get; set; }
			DateTime? DateOfBirth { get; set; }
			DateTime? CreatedFrom { get; set; }
			DateTime? CreatedTo { get; set; }
		}
	}

Now we can essentially extend the parameters of the StudentFilterQuery by inheriting the PaginationQuery, have a look above at the IStudentFilter implementation. Try inheriting in a similar fashion on the classes. This saves us writing out the same variables over and over for every query class and means we’re consistent in naming. Now make sure the StudentFilter model is passed all the way down to the repository level. 

Now we’re down here, we’re going to make sure we can process pagination against every database table in the same way. Firstly, add a PaginationExtensions call into a QueryExtensions folder. The class to implement is as follows… 

	namespace Repositories.QueryExtensions
	{
		public static class PaginationExtensions { 

			public static PaginationResponse GetPaginationResponse(int numberPerPage, int pageNumber, int totalRecords, bool sortByDecending = true, string sortBy = "")
			{
				var response = new PaginationResponse
				{
					NumberOfPages = ((totalRecords + numberPerPage - 1) / numberPerPage),
					TotalRecords = totalRecords,
					DisplayedPerPage = numberPerPage,
					SortBy = sortBy,
					SortByDecending = sortByDecending
				};

				if (pageNumber == 0) {
					response.PageNumber = 1;
				} 
				else
				{
					if(pageNumber > response.NumberOfPages)
					{
						response.PageNumber = response.NumberOfPages == 0 ? 1 : response.NumberOfPages;
					} else
					{
						response.PageNumber = pageNumber;
					}
				}

				return response;
			}

			public static IQueryable<T> ApplyPagination<T>(IQueryable<T> query, int displayedPerPage, int pageNumber)
			{
				var offset = displayedPerPage * (pageNumber - 1);
				var takeAmount = displayedPerPage;

				query = query.Skip(offset);
				query = query.Take(takeAmount);

				return query;
			}

			public static IList<T> ApplyPagination<T>(IEnumerable<T> query, int displayedPerPage, int pageNumber)
			{
				var offset = displayedPerPage * (pageNumber - 1);
				var takeAmount = displayedPerPage;

				query = query.Skip(offset);
				query = query.Take(takeAmount);

				return query.ToList();
			}

			public static IQueryable<IGrouping<T,S>> ApplyPagination<T,S>(IQueryable<IGrouping<T, S>> query, int displayedPerPage, int pageNumber)
			{
				var offset = displayedPerPage * (pageNumber - 1);
				var takeAmount = displayedPerPage;

				query = query.Skip(offset);
				query = query.Take(takeAmount);

				return query;
			}
		}
	}

Now one thing to note on this class is that it uses the static accessor. Static means that we do not have to create the class to use its functions! We can also see instances of templating in the form of <T> declarations. Templating means we can write one function and apply it to multiple data types, in this case, our tables. We won’t go into too much more detail on this in the Dojo but it is something worth exploring at a later date when you are more familiar with the features of C#. 

Over to our StudentsRepostory, on our GetStudents function, try implementing our new PaginationExtensions function ApplyPagination at the right point in code...

	PaginationResponse paginationResponse = null;
	if(query.DisplayedPerPage != 0 && query.PageNumber != 0)
	{
		var totalRecords = await studentsQuery.CountAsync();
		paginationResponse = PaginationExtensions.GetPaginationResponse(query.DisplayedPerPage, query.PageNumber, totalRecords, query.SortByDecending);
		studentsQuery = PaginationExtensions.ApplyPagination(studentsQuery, paginationResponse.DisplayedPerPage, paginationResponse.PageNumber);
	}

If done correctly, pagination should be applied seamlessly from the Swagger UI. Have a look over the code, throw in some breakpoints if you need to analyse the process in depth but this pagination code is an extremely good standard and start to dynamic LINQ processing. The completed code can be found in “7 – Core Dojo – Filtering with Inheritance”

***AutoMapper***

You'll probably have noticed that we've had to manually create our StudentResponse object from our Student entity. As you can imagine, manually doing this mapping for every Entity to DTO will become a huge pain. It's also difficult to remember to make the appropriate changes to this mapping whenever these objects change. So to avoid doing this, we use a NuGet package called AutoMapper which does it all for you!

Let's install the required NuGet packages
* Install the latest version of AutoMapper to the common project
* Install the latest version of AutoMapper.Extensions.Microsoft.DependencyInjection to the Core Dojo project

The next step is to tell AutoMapper how we want to map our objects to one another using Profiles. Let's create a new folder inside the Repositories project and call it AutoMapperProfiles. Then create a new class called StudentProfile with the following code

```csharp
public class StudentProfile : Profile
{
	public StudentProfile()
	{
		CreateMap<Student, StudentResponse>();
	}
}
```

AutoMapper is clever enough to map properties that match and so this is all you need to do to setup a mapping from Student to StudentResponse.

We now need to configure AutoMapper so that it can find these profiles and register the AutoMapper service. To do this we need to go back to the Startup class in Core Dojo and the following to the ConfigureServices method

```csharp
// Setup AutoMapper
var mapperConfig = new MapperConfiguration(cfg => cfg.AddMaps(typeof(StudentProfile).Assembly));
IMapper mapper = mapperConfig.CreateMapper();
services.AddSingleton(mapper);
```

The AddMaps takes in an array of assemblies and looks for all AutoMapper Profiles in there to use when mapping. We've told it to look for Profiles in the Assembly that contains the StudentProfile. This means it'll pick all the other Profiles in that folder and not just StudentProfile. We then create our mapper and register it so it can be DI'd in our solution.

We are now ready to use the mapper, so let's open the StudentsRepository class and add it.

```csharp
private readonly RepositoryContext _context;
private readonly IMapper _mapper;

public StudentsRepository(RepositoryContext context, IMapper mapper)
{
	_context = context;
	_mapper = mapper;
}
```

Our mapper is now available in the StudentsRepository and we can use it to map our Student entity to a StudentResponse. We're currently manually doing this in the GetStudent and GetStudents methods. All we have to do now is replace the manual mapping to

```csharp
var studentResponse = _mapper.Map<StudentResponse>(student);
```

and 

```csharp
var studentsResponse = _mapper.Map<List<StudentResponse>>(students);
```

Much better! As you can see, AutoMapper is capable of mapping lists of objects too.

Let's make sure it works, so run the application and if you haven't done so already then create a new student in swagger. Then call both the API methods to get your students and see if it returns what you expected.

AutoMapper is a very powerful and configurable tool and you can set up very complex profiles if you need to. Let's try to see how we'd go about mapping a 

***Service Level Validation and Logic***

Up until now, we've been coding in a way that assumes that everything is going to be fine and we're always going to receive the data we expect to. This is a great way to run into issues and if we don't do anything to protect against it then we're going to run into all sorts of problems. The solution to this is [defensive programming](https://en.wikipedia.org/wiki/Defensive_programming).

The best place for us to start is the AddStudent method in the StudentsService as this is where we will be receiving untrusted data from external clients. If you look at the method signature, you can see we're expecting a StudentAddRequest. We know we want to access properties on this object, so let's check the object actually exists and isn't null.

```csharp
if (request == null)
{
	throw new ArgumentNullException(nameof(request));
}
```

We do this by adding something called a **Guard Clause** which checks for an error condition and throws the appropriate exception to stop the code from running into unhandled errors in the main logic of the method. This way of spotting errors and throwing an exception early is known as **Fail Fast**. You want to fail fast so that you can see exactly where and when an error occurred in the code to better understand what has happened. There's no point letting the code execute when there's an error as it'll eventually fail somewhere else and it won't always be as clear why.

Earlier, we created a configuration which specified which columns were mandatory and the max length of them. So let's add those guard clauses here before it ever reaches the database.

```csharp
if (string.IsNullOrWhiteSpace(request.FirstName))
{
	throw new ArgumentException($"{nameof(request.FirstName)} is missing.");
}

if (request.FirstName.Length > 50)
{
	throw new ArgumentException($"{nameof(request.FirstName)} is too long.");
}

if (request.MiddleNames != null && request.MiddleNames.Length > 200)
{
	throw new ArgumentException($"{nameof(request.MiddleNames)} is too long.");
}

if (string.IsNullOrWhiteSpace(request.Surname))
{
	throw new ArgumentException($"{nameof(request.Surname)} is missing.");
}

if (request.Surname.Length > 50)
{
	throw new ArgumentException($"{nameof(request.Surname)} is too long.");
}
```

Now we can be sure that if the code executes past these clauses then the request data is one we're happy with and can add it to the database. There's nothing checking for DateOfBirth yet so try and come up with some sensible guard clauses for it.

Let's see what happens when we hit these guard clauses. Run the application and try to add a student that has breaks our rules. You'll see that the server responded with our error message but it also spewed out a bunch of extra internal information to the user and gave us a 500 error. This isn't good as you're giving the client information that it doesn't or shouldn't care about and possibly giving clues to malicious actors. The 500 error also means something unexpected happened on the server and it wasn't the user's fault that it happened. 

**Try Catch**

We've thrown these exceptions in our service but they're not being handled anywhere. Normally we'd use a middleware to handle these service level exceptions but let's do it in the controller for now. So the exceptions are thrown whenever we run into a condition we're not happy with and we want to somehow let the user know what they've done wrong. This is what try catches are for. They attempt to do something within the try block and if it goes wrong then it goes into the catch block and executes that code before continuing.

Change the SaveStudent method in the StudentsController so that it now has the try catch.

```csharp
try
{
	await _studentsService.AddStudent(request);
}
catch
{
	return BadRequest();
}

return Ok();
```

This code is going to try and add the student and if there's an error, it'll return a BadRequest (400) response to the user meaning they've done something wrong. Run the application and try to add a student with errors again to see what happens. This time, we got a neater response and have received the 400 error as expected except now we've lost our message to the user to let them know why this has happened!

```csharp
try
{
	await _studentsService.AddStudent(request);
}
catch(Exception e)
{
	return BadRequest(e.Message);
}

return Ok();
```

We can actually catch the exception that was thrown earlier and take the message from it to send in our BadRequest and show it to the client. Try this instead and you should now see that you have the 400 error with the message from the exception that was thrown.

There is a problem with this try catch as it'll catch every exception that could possibly be thrown in AddStudent and tell the client it was their fault! This won't always be the case so let's only catch the ones we care about so that we can be sure we're sending the right responses to the client.

```csharp
try
{
	await _studentsService.AddStudent(request);
}
catch(Exception e)
{
	return BadRequest(e.Message);
}

return Ok();
```

Now try throwing a generic exception in the AddStudent method and you'll see it gets treated differently to the ArgumentExceptions. 

There's a few more things you can do with try catch such as [try catch finally](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/try-catch-finally) and try catch when, and rethrowing the exception.

**try catch when**
```csharp
try
{
	await _studentsService.AddStudent(request);
}
catch(ArgumentException e) when (e.Message.Contains("FirstName"))
{
	return BadRequest(e.Message);
}
```
We only catch the exception when the exception message contains FirstName.

```csharp
try
{
	// Some code that could throw
}
catch(Exception e)
{
	// Log the exception e here but then throw again
	throw;
}
```
Here we don't actually want to do anything except log that the exception has happened and we want the exception to continue bubbling up undisturbed. We deliberately don't do `throw e;` here as this would destroy the stack trace information.


**Cutting Edge Conditions** - Nuget Package 

**IValidation method** - Optional


 ***Unit Testing***

We've been writing code and running it to test what happens. This is a good way to see what happens end to end quickly but it's not easily repeatable or reliable, and would be time consuming to check everything still works as expected. This is why we write unit tests. They assert that the code in a small unit (usually a class or method) is behaving exactly as we expect whenever we make changes to our code. 

To start unit testing, we need to create a new project which we'll call Tests. Then we need to install the xunit and xunit.runner.visualstudio NuGet packages to the project.

Now we mimic the project and folder structure of the solution within the Tests project. Create a new Folder called Core Dojo for the Core Dojo project tests. Inside that, create a new folder called services. Finally, create a new class called StudentsServiceTests and copy the below code into it

```csharp
public class StudentsServiceTests
{
	[Fact]
	public async Task AddStudent_WhenAddStudentRequestIsNull_ShouldThrowArgumentNullException()
	{
		// Arrange
		var service = new StudentsService(null, null);
		StudentAddRequest student = null;

		// Act
		Func<Task<bool>> act = () => service.AddStudent(student);

		// Assert
		var exception = await Assert.ThrowsAsync<ArgumentNullException>(act);
		Assert.Contains("request", exception.Message);
	}
}
```

There's a lot happening in the above block of code, so let's break it down.

To let xunit and Visual Studio know which method is a test we have to add a `[Fact]` attribute to it. This means it'll get picked up by the test runner and you'll now be able to run this test method.

The method name `AddStudent_WhenAddStudentRequestIsNull_ShouldThrowArgumentNullException()` is written in a special way by convention to make it simple to see what should be happening and the structure we use is MethodUnderTest_Scenario_Result.

The method itself is split into 3 sections known as Arrange, Act, Assert (AAA). This AAA pattern is a common way of writing unit tests. The Arrange section is where you set up all the data you will need when calling the method under test and checking the result. This will have all your variables. The Act section is the part where we call the method we are testing and is usually one line. The Assert section is checking that given the scenario we have set up, the result is what we expected it to be.

In the Arrange part, we set up our service so that we can call the methods on it. To keep it simple, I've not given the service its dependencies as they're not strictly necessary for this test. We then have to set up the argument that we're passing in to the AddStudent method. As you can see from the method name, we want to test what happens when we give the AddStudent method a null StudentAddRequest.

Now the Act line is a bit special in this case as we're setting up a variable called act to delay the call the AddStudent call. We do this here because if we were to call the AddStudent method directly then it would throw the exception and the test would fail as it wasn't expecting the exception to be thrown yet. We are storing the act in a `Func<Task<bool>>` as it is a method which is supposed to return us a boolean wrapped in a Task. Remember, it's the result is wrapped in a Task because AddStudent is an async method.

Finally, the assert section is where we check the result is as expected and xunit provides us with some tools to help us do this. `Assert.ThrowsAsync<T>` is a method which checks that when the act is called then it should throw an Exception of type T. In our case, we know that if we give the AddStudent a null request then it should throw an ArgumentNullException. Therefore T is ArgumentNullException here. This Assert method is pretty useful as it also returns us the exception so that we can examine it and make additional Assertions if needed. In this example, we're checking the exception message contains the word request somewhere in it.

Have a go at writing the tests for the other guard clauses.

**Happy Path and Mocking**

So now that we've tested all our failure paths, we can now test that the method does what it's supposed to when we have a valid request. We're going to need to give the service a dependency now as the AddStudent method uses the Repository to add a student to the database. But I mentioned earlier that we only want to test the class or method as a unit and nothing else. We do this by mocking the dependency.

We need to install a NuGet package called Moq to the Tests project. Once that's done, we're ready to start mocking. Moq takes advantage of the fact that we're using interfaces to DI our dependencies. It knows what the real dependency needs to look like and provides us with a fake one which we're free to manipulate so that it gives us what we want.


```csharp
[Fact]
public async Task AddStudent_WhenValidRequest_ShouldReturnBoolean()
{
	// Arrange
	var studentsRepositoryMock = new Mock<IStudentsRepository>();
	var service = new StudentsService(studentsRepositoryMock.Object, null);
	var student = new StudentAddRequest
	{
		FirstName = "Joe",
		MiddleNames = "Fred",
		Surname = "Bloggs",
		DateOfBirth = new DateTime(1980, 10, 24)
	};

	studentsRepositoryMock.Setup(sr => sr.AddStudent(student)).ReturnsAsync(true);

	// Act
	var result = await service.AddStudent(student);

	// Assert
	Assert.True(result);

    studentsRepositoryMock.Verify(sr => sr.AddStudent(student), Times.Once);
}
```

All we need to do to create a mock is to just give it the interface of the type we want mocked. The mock stores the instance of the object which the StudentsService requires on a property called Object. We pass this object to the service so that it can be used in the AddStudent method. We then setup a a valid student request to give to the AddStudent method we're testing.

The last step of the Arrange is to setup our mock StudentRepository so that it does what we tell it to do. `studentsRepositoryMock.Setup(sr => sr.AddStudent(student)).ReturnsAsync(true);` This is saying that whenever an AddStudent call is made on the repository with the student argument, then it should return true. The student argument here is the exact variable we setup earlier. If it's anything else, then it has not been setup to return true. Try making a copy of this test with student1 and student2 where the values are exactly the same. Set up the mock for student1 but pass student2 to the AddStudent service to see what happens.

In the act, we get a value back from the AddStudent method which we want to check. We have just setup the AddStudent repository method to return true using our mock. We know that the AddStudent service method just returns this value directly from the repository so that's what we're testing. 

Once we have our result, we use the `Assert.True` method which simply checks that the value of a variable is true, and fails the test if it isn't. Afterwards, we can verify that a call to the repository AddStudent method actually happened and how many times it happened.