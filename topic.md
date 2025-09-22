Model Binding 的核心内容就是请求的 validation。
根据不同的请求来做不一样的请求验证处理，这就是这节课的核心。

FromRoute 相关的 Model Binding 特性：

## 1. 基础路由绑定

- [FromRoute] - 从路由参数绑定
- 路由模板: {id}, {name}, {category} 等

## 2. 路由参数类型

- string - 字符串参数
- int, long, decimal - 数值参数
- bool - 布尔参数 (true/false, 1/0)
- DateTime - 日期时间参数
- Guid - 全局唯一标识符
- enum - 枚举类型

## 3. 路由约束 (Route Constraints)

- {id:int} - 整数约束
- {id:min(1)} - 最小值约束
- {id:max(100)} - 最大值约束
- {id:range(1,100)} - 范围约束
- {name:length(5)} - 固定长度约束
- {name:minlength(2)} - 最小长度约束
- {name:maxlength(50)} - 最大长度约束
- {email:regex(^\w+@\w+\.\w+$)} - 正则表达式约束
- {date:datetime} - 日期时间约束
- {id:guid} - GUID 约束
- {category:alpha} - 字母约束
- {code:alphanumeric} - 字母数字约束

## 4. 可选参数

- {id?} - 可选参数
- {category?} - 可选字符串参数
- 默认值: {page:int=1} - 带默认值的参数

## 5. 捕获所有参数

- {\*path} - 捕获剩余路径
- {\*\*path} - 捕获包含斜杠的路径

## 6. 复合约束

- {id:int:min(1):max(1000)} - 多个约束组合
- {name:length(5):alpha} - 长度和字母约束

## 7. 自定义路由约束

- 实现 IRouteConstraint 接口
- 注册自定义约束

## 8. 模型绑定选项

- [FromRoute(Name = "customName")] - 自定义参数名映射
- 绑定复杂对象的属性

## 9. 验证特性 (配合使用)

- [Required] - 必需参数
- [Range(1, 100)] - 范围验证
- [StringLength(50)] - 字符串长度验证
- [RegularExpression] - 正则表达式验证

### FromRoute 代码示例：

#### 1. Required 验证 - 必需参数

```csharp
[HttpGet("required/{name}")]
public IActionResult RequiredValidation([FromRoute][Required] string name)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }
    return Ok(new { name, message = "必需参数验证通过" });
}
```

#### 2. Range 验证 - 范围验证

```csharp
[HttpGet("range/{age:int}")]
public IActionResult RangeValidation([FromRoute][Range(18, 100)] int age)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }
    return Ok(new { age, message = "年龄必须在18-100之间" });
}
```

#### 3. StringLength 验证 - 字符串长度

```csharp
[HttpGet("stringlength/{username}")]
public IActionResult StringLengthValidation(
    [FromRoute][StringLength(20, MinimumLength = 3)] string username)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }
    return Ok(new { username, message = "用户名长度3-20字符" });
}
```

#### 4. RegularExpression 验证 - 正则表达式

```csharp
[HttpGet("regex/{email}")]
public IActionResult RegexValidation(
    [FromRoute]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
                      ErrorMessage = "邮箱格式不正确")]
    string email)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }
    return Ok(new { email, message = "邮箱格式验证通过" });
}
```

#### 5. 组合验证 - 多个验证特性

```csharp
[HttpGet("combined/{id:int}/{name}/{email}")]
public IActionResult CombinedValidation(
    [FromRoute][Range(1, 1000)] int id,
    [FromRoute][Required][StringLength(50, MinimumLength = 2)] string name,
    [FromRoute][RegularExpression(@"^[\w\.-]+@[\w\.-]+\.\w+$")] string email)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(new {
            errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
        });
    }

    return Ok(new { id, name, email, message = "所有验证通过" });
}
```

#### 6. 自定义错误消息

```csharp
[HttpGet("custom-message/{score:decimal}")]
public IActionResult CustomErrorMessage(
    [FromRoute]
    [Range(0.0, 100.0, ErrorMessage = "分数必须在0-100之间")]
    decimal score)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }
    return Ok(new { score, message = "分数验证通过" });
}
```

#### 7. 复杂模型验证

```csharp
public class UserModel
{
    [FromRoute(Name = "userId")]
    [Range(1, int.MaxValue, ErrorMessage = "用户ID必须大于0")]
    public int UserId { get; set; }

    [FromRoute(Name = "username")]
    [Required(ErrorMessage = "用户名不能为空")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "用户名长度必须在3-20字符之间")]
    public string Username { get; set; } = string.Empty;
}

[HttpGet("model/{userId:int}/{username}")]
public IActionResult ModelValidation([FromRoute] UserModel model)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }
    return Ok(new { model, message = "模型验证通过" });
}
```

---

# FromBody 相关的 Model Binding 特性

FromBody 用于从 HTTP 请求体中绑定数据，主要用于 POST、PUT、PATCH 等请求。

## 1. 基础 FromBody 绑定

- [FromBody] - 从请求体绑定数据
- 支持 JSON、XML 等格式
- 通常用于复杂对象传输

## 2. 支持的数据格式

- **JSON** - 最常用，Content-Type: application/json
- **XML** - Content-Type: application/xml
- **Form Data** - Content-Type: application/x-www-form-urlencoded
- **Plain Text** - Content-Type: text/plain

## 3. FromBody 约束和特性

- **一个 Action 只能有一个 [FromBody] 参数**
- **必须是 POST、PUT、PATCH 等有请求体的方法**
- **自动反序列化 JSON/XML 到 C# 对象**
- **支持复杂嵌套对象**

## 4. 数据验证特性 (与 FromBody 配合)

- [Required] - 必需字段
- [Range] - 数值范围验证
- [StringLength] - 字符串长度验证
- [EmailAddress] - 邮箱格式验证
- [Phone] - 电话号码验证
- [Url] - URL 格式验证
- [CreditCard] - 信用卡号验证
- [Compare] - 字段比较验证
- [RegularExpression] - 正则表达式验证

## 5. 自定义验证特性

- 继承 ValidationAttribute
- 实现 IsValid 方法
- 支持复杂业务逻辑验证

### FromBody 代码示例：

#### 1. 基础 JSON 绑定

```csharp
public class CreateUserRequest
{
    [Required(ErrorMessage = "用户名不能为空")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "用户名长度必须在3-50字符之间")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "邮箱不能为空")]
    [EmailAddress(ErrorMessage = "邮箱格式不正确")]
    public string Email { get; set; } = string.Empty;

    [Range(18, 120, ErrorMessage = "年龄必须在18-120之间")]
    public int Age { get; set; }
}

[HttpPost("create-user")]
public IActionResult CreateUser([FromBody] CreateUserRequest request)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    return Ok(new {
        message = "用户创建成功",
        user = request,
        timestamp = DateTime.Now
    });
}
```

**测试 JSON:**

```json
{
  "username": "john_doe",
  "email": "john@example.com",
  "age": 25
}
```

#### 2. 嵌套对象验证

```csharp
public class Address
{
    [Required(ErrorMessage = "街道地址不能为空")]
    [StringLength(100, ErrorMessage = "街道地址不能超过100字符")]
    public string Street { get; set; } = string.Empty;

    [Required(ErrorMessage = "城市不能为空")]
    [StringLength(50, ErrorMessage = "城市名不能超过50字符")]
    public string City { get; set; } = string.Empty;

    [RegularExpression(@"^\d{6}$", ErrorMessage = "邮政编码必须是6位数字")]
    public string PostalCode { get; set; } = string.Empty;
}

public class CreateEmployeeRequest
{
    [Required(ErrorMessage = "员工姓名不能为空")]
    [StringLength(50, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "员工邮箱不能为空")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Range(1000, 100000, ErrorMessage = "薪资必须在1000-100000之间")]
    public decimal Salary { get; set; }

    [Required(ErrorMessage = "地址信息不能为空")]
    public Address Address { get; set; } = new();

    [Phone(ErrorMessage = "电话号码格式不正确")]
    public string? Phone { get; set; }
}

[HttpPost("create-employee")]
public IActionResult CreateEmployee([FromBody] CreateEmployeeRequest request)
{
    if (!ModelState.IsValid)
    {
        var errors = ModelState
            .Where(x => x.Value.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            );

        return BadRequest(new { message = "验证失败", errors });
    }

    return Ok(new {
        message = "员工创建成功",
        employee = request
    });
}
```

**测试 JSON:**

```json
{
  "name": "张三",
  "email": "zhangsan@company.com",
  "salary": 8000,
  "address": {
    "street": "中关村大街1号",
    "city": "北京",
    "postalCode": "100084"
  },
  "phone": "13800138000"
}
```

#### 3. 集合验证

```csharp
public class BatchCreateUsersRequest
{
    [Required(ErrorMessage = "用户列表不能为空")]
    [MinLength(1, ErrorMessage = "至少需要一个用户")]
    [MaxLength(10, ErrorMessage = "一次最多创建10个用户")]
    public List<CreateUserRequest> Users { get; set; } = new();

    [Required(ErrorMessage = "操作者不能为空")]
    public string CreatedBy { get; set; } = string.Empty;

    public string? Remarks { get; set; }
}

[HttpPost("batch-create-users")]
public IActionResult BatchCreateUsers([FromBody] BatchCreateUsersRequest request)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    // 验证用户名唯一性（自定义业务逻辑）
    var usernames = request.Users.Select(u => u.Username).ToList();
    var duplicates = usernames.GroupBy(x => x)
        .Where(g => g.Count() > 1)
        .Select(g => g.Key);

    if (duplicates.Any())
    {
        return BadRequest(new {
            message = "用户名重复",
            duplicateUsernames = duplicates
        });
    }

    return Ok(new {
        message = $"成功创建{request.Users.Count}个用户",
        users = request.Users,
        createdBy = request.CreatedBy
    });
}
```

**测试 JSON:**

```json
{
  "users": [
    {
      "username": "user1",
      "email": "user1@example.com",
      "age": 25
    },
    {
      "username": "user2",
      "email": "user2@example.com",
      "age": 30
    }
  ],
  "createdBy": "admin",
  "remarks": "批量导入用户"
}
```

#### 4. 自定义验证特性

```csharp
public class MinAgeAttribute : ValidationAttribute
{
    private readonly int _minAge;

    public MinAgeAttribute(int minAge)
    {
        _minAge = minAge;
        ErrorMessage = $"年龄必须大于等于{minAge}岁";
    }

    public override bool IsValid(object? value)
    {
        if (value is DateTime birthDate)
        {
            var age = DateTime.Today.Year - birthDate.Year;
            if (birthDate.Date > DateTime.Today.AddYears(-age))
                age--;

            return age >= _minAge;
        }
        return false;
    }
}

public class UserProfileRequest
{
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MinAge(18)] // 自定义验证：必须年满18岁
    public DateTime BirthDate { get; set; }

    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Url(ErrorMessage = "个人网站URL格式不正确")]
    public string? Website { get; set; }
}

[HttpPost("update-profile")]
public IActionResult UpdateProfile([FromBody] UserProfileRequest request)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    var age = DateTime.Today.Year - request.BirthDate.Year;
    if (request.BirthDate.Date > DateTime.Today.AddYears(-age))
        age--;

    return Ok(new {
        message = "用户资料更新成功",
        profile = request,
        calculatedAge = age
    });
}
```

**测试 JSON:**

```json
{
  "name": "李四",
  "birthDate": "1990-05-15",
  "email": "lisi@example.com",
  "website": "https://lisi.dev"
}
```

#### 5. 条件验证

```csharp
public class OrderRequest
{
    [Required]
    [StringLength(100)]
    public string ProductName { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "数量必须大于0")]
    public int Quantity { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "价格必须大于0")]
    public decimal Price { get; set; }

    public bool IsGift { get; set; }

    // 如果是礼品，收件人信息必填
    public string? RecipientName { get; set; }
    public string? RecipientAddress { get; set; }
    public string? GiftMessage { get; set; }
}

[HttpPost("create-order")]
public IActionResult CreateOrder([FromBody] OrderRequest request)
{
    // 自定义验证逻辑
    if (request.IsGift)
    {
        if (string.IsNullOrWhiteSpace(request.RecipientName))
        {
            ModelState.AddModelError(nameof(request.RecipientName),
                "礼品订单必须填写收件人姓名");
        }

        if (string.IsNullOrWhiteSpace(request.RecipientAddress))
        {
            ModelState.AddModelError(nameof(request.RecipientAddress),
                "礼品订单必须填写收件人地址");
        }
    }

    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    var totalPrice = request.Price * request.Quantity;

    return Ok(new {
        message = "订单创建成功",
        order = request,
        totalPrice = totalPrice
    });
}
```

**测试 JSON (礼品):**

```json
{
  "productName": "生日蛋糕",
  "quantity": 1,
  "price": 299.0,
  "isGift": true,
  "recipientName": "王五",
  "recipientAddress": "上海市浦东新区张江路1号",
  "giftMessage": "生日快乐！"
}
```

#### 6. 文件上传与表单混合

```csharp
public class UploadFileRequest
{
    [Required(ErrorMessage = "文件名不能为空")]
    [StringLength(100)]
    public string FileName { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [Required(ErrorMessage = "文件内容不能为空")]
    public string FileContent { get; set; } = string.Empty; // Base64 编码的文件内容

    [Required(ErrorMessage = "文件类型不能为空")]
    [RegularExpression(@"^(jpg|jpeg|png|gif|pdf|doc|docx)$",
        ErrorMessage = "不支持的文件类型")]
    public string FileType { get; set; } = string.Empty;
}

[HttpPost("upload-file")]
public IActionResult UploadFile([FromBody] UploadFileRequest request)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    try
    {
        // 验证 Base64 格式
        var fileBytes = Convert.FromBase64String(request.FileContent);
        var fileSizeKB = fileBytes.Length / 1024;

        if (fileSizeKB > 5120) // 5MB 限制
        {
            return BadRequest(new { message = "文件大小不能超过5MB" });
        }

        return Ok(new {
            message = "文件上传成功",
            fileName = request.FileName,
            fileSize = $"{fileSizeKB}KB",
            fileType = request.FileType
        });
    }
    catch (FormatException)
    {
        return BadRequest(new { message = "文件内容格式错误" });
    }
}
```

### 测试工具和示例：

#### 使用 curl 测试：

```bash
# 创建用户
curl -X POST "http://localhost:5023/api/[Controller]/create-user" \
  -H "Content-Type: application/json" \
  -d '{
    "username": "test_user",
    "email": "test@example.com",
    "age": 25
  }'

# 批量创建用户
curl -X POST "http://localhost:5023/api/[Controller]/batch-create-users" \
  -H "Content-Type: application/json" \
  -d '{
    "users": [
      {"username": "user1", "email": "user1@test.com", "age": 20},
      {"username": "user2", "email": "user2@test.com", "age": 25}
    ],
    "createdBy": "admin"
  }'
```

#### 常见验证错误示例：

❌ **验证失败的情况：**

1. **必填字段为空**
2. **邮箱格式错误**
3. **数值超出范围**
4. **字符串长度不符合要求**
5. **嵌套对象验证失败**
6. **自定义验证逻辑失败**

### FromBody vs FromRoute vs FromQuery 对比：

| 特性     | FromRoute              | FromQuery              | FromBody         |
| -------- | ---------------------- | ---------------------- | ---------------- |
| 数据来源 | URL 路径               | URL 查询字符串         | 请求体           |
| 适用方法 | GET, POST, PUT, DELETE | GET, POST, PUT, DELETE | POST, PUT, PATCH |
| 数据类型 | 简单类型               | 简单类型               | 复杂对象         |
| 数据大小 | 有限                   | 有限                   | 大量数据         |
| 缓存友好 | 是                     | 是                     | 否               |
| SEO 友好 | 是                     | 部分                   | 否               |

## 10. 高级 FromBody 特性

- 自定义模型绑定器 (Custom Model Binders)
- 输入格式化器 (Input Formatters)
- 模型绑定元数据 (Model Binding Metadata)
- 复杂类型转换器 (Type Converters)
- 异步验证 (Async Validation)
