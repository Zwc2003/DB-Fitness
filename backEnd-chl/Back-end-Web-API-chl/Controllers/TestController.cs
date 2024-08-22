using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Back_end_Web_API_chl.Controllers
{
    //[Route("[controller]")] //此种方式遵循restful风格，路由不展示具体调用函数名（即接口的功能，通过请求方法来区分：get,post,put,delete），前端无论哪个方法接口，路由均统一为这个类名
    //[Route("[controller]/[action]")] //该方式对于每个不同的方法，路由中的[action]将被替换为各自的函数名
    [Route("[controller]/[action]")]  //路由用于指引前端找到我们的数据
    [ApiController]
    public class TestController : ControllerBase
    {

        [HttpGet]
        public string Get1(string user_name,string password)  //可以在这里传递参数，即后端传递，可以防止使用表单传输时，重要数据的前端显示
        {
            return "这里是控制器创建测试1-程鸿磊";
        }

        //从后端获取数据的方法：使用JDBC；使用ORM

        //需要解决SQL注入：查一下ORACLE的解决方法（SQL server是通过参数化来解决）

        //数据库获取数据的封装使用：创建sqlHelper类：初始化连接字符串；传入SQL语句以及需要的参数即可使用
        //工具类不要通过new一个实例的方式来使用，做成一个static直接用类名使用更合理；eg:对于SQL的增删改查实现的方法类

        //视图模型与领域模型：视图模型是实际用到的变量，领域模型是与数据库直接交互的变量，它包含的更全，一般是一个表中的所有属性
        //当与数据库发生交互时，需要将视图模型向领域模型进行转化；同理，从数据库中抽取数据时也是；
        //实现方式是创建一个表的所有属性的中间变量进行暂存，使用中间变量进行交互，最后统一更新
        //从而引入Model类（Model文件夹下创建）

        //WEB API需要尽可能减少底层的细节，只需要业务逻辑(调用方法)，不需要实现过程
        //创建数据访问层（Date Access Layer , DAL）的封装,每个Model各自的DAL类用于和数据库的交互
        //数据访问层一般只用于获取数据，不对数据进行进一步筛选

        //经典三层架构：WebAPI层（UI层） 业务逻辑层（BLL） 数据访问层（DAL）
        //创建业务逻辑层（BLL）的封装，（BLL文件夹下）
        //对于登录来讲：删除分为软删除和硬删除，软删除通过修改字段的值来标记该元组无效，硬删除即直接删除；一般需求均为软删除，软删除的实质是更新

        //方法命名：动词在前名词在后 首字母大写

        //CRUD程序员-》架构设计程序员  不要做CV程序员//

        //总的来说，Model类用于定义数据结构， DAL直接与数据库发生关系，BLL调用DAL类的方法来很具体实现业务逻辑， Controller类调用BLL类完成接口功能

        //RESTful风格：整体路由为[Route("[controller]")]，内部不同方法的区分通过HTTP请求方法+HTTP请求的路由（可以通过传递参数来实现）来区分 


        //高内聚 低耦合： 相同业务之间可以高内聚，只是要借用方法的类之间要低耦合；通过解耦合的方式来实现低耦合
        //解耦合的具体方法是面向抽象/依赖倒置（即依赖于抽象，不依赖于BLL类），抽象即代表接口（Interface:内部拥有未实现的一组方法）
        //接口可以起到规范开发的作用：它是一种约束；另外通过一个统一的接口可以实现不同的功能，即多态；层间引用？
        //在BLL文件夹下创建一个Interfaces文件夹用于储存不同的接口类（Interface），这个类只声明有哪些方法而不实现，具体的BLL类通过继承自己的IBLL类，在内部实现那方法

        //WebAPI和BLL层的解耦合： 在Web API层不需要new一个BLL的实例就可以调用BLL类的方法；
        //实现：.NET Core提供了依赖注入来实现上面的过程，在Program.cs中注入BLL类，有三种方式：
        //Services.AddSingleton() //注入单例，创建后一直在内存中保留不再注销
        //Services.AddScoped() //注入单例,一次请求中不被注销
        //Services.AddTransient() //瞬时，即用即删
        //在Controller类的构造函数中传入的参数为所需的BLL类，并赋值给Controller类的私有成员变量_BLL,之后通过_BLL来使用即可
        //原理：当program.cs中ADD了相应的BLL类后，系统会会将这个构造好的实例添加到一个容器中，每次Controller类构造函数调用时会在这个容器中检索所需的类并放到Controller中

        //经典三层架构：WebAPI层（UI层） 业务逻辑层（BLL） 数据访问层（DAL）
        //VS中创建一个新项目，类型为类库

        //循环引用形成闭环在开发中是不被允许的

    }
}
