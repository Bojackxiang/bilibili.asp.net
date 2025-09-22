# FromRoute 测试端点

## 基础使用

```
GET /api/RouteExamples/basic/123
GET /api/RouteExamples/user/1/post/5
```

## 不同数据类型

```
GET /api/RouteExamples/types/123/张三/true/2024-12-25
```

## 路由约束

```
GET /api/RouteExamples/constraints/500      # ✅ 成功 (1-1000范围内)
GET /api/RouteExamples/constraints/1500     # ❌ 失败 (超出范围)
```

## 可选参数

```
GET /api/RouteExamples/optional/技术           # page使用默认值
GET /api/RouteExamples/optional/技术/2          # 指定page=2
```

## 默认值参数

```
GET /api/RouteExamples/default/新闻            # page=1 (默认)
GET /api/RouteExamples/default/新闻/3          # page=3
```

## 字符串长度约束

```
GET /api/RouteExamples/length/ABC12         # ✅ 成功 (5位长度)
GET /api/RouteExamples/length/AB            # ❌ 失败 (长度不足)
```

## 正则表达式约束

```
GET /api/RouteExamples/regex/user@example.com    # ✅ 成功
GET /api/RouteExamples/regex/invalid-email       # ❌ 失败
```

## GUID 约束

```
GET /api/RouteExamples/guid/550e8400-e29b-41d4-a716-446655440000
```

## 捕获所有参数

```
GET /api/RouteExamples/catchall/path/to/resource/file.txt
```

## 枚举类型

```
GET /api/RouteExamples/enum/Pending          # ✅ 成功
GET /api/RouteExamples/enum/Processing       # ✅ 成功
GET /api/RouteExamples/enum/InvalidStatus    # ❌ 失败
```

## 复合模型绑定

```
GET /api/RouteExamples/model/123/delete
```

## 验证特性

```
GET /api/RouteExamples/validated/500/张三     # ✅ 成功
GET /api/RouteExamples/validated/1500/张三    # ❌ 失败 (ID超出范围)
GET /api/RouteExamples/validated/500/很长的名字超过五十个字符限制测试    # ❌ 失败 (名字太长)
```

## 复杂约束组合

```
GET /api/RouteExamples/complex/100/ABC/85.5   # ✅ 成功
GET /api/RouteExamples/complex/0/ABC/85.5     # ❌ 失败 (ID<1)
GET /api/RouteExamples/complex/100/AB1/85.5   # ❌ 失败 (code包含数字)
GET /api/RouteExamples/complex/100/ABC/150.0  # ❌ 失败 (分数>100)
```

## 验证特性详细测试

### Required 验证

```
GET /api/RouteExamples/validation/required/张三    # ✅ 成功
GET /api/RouteExamples/validation/required/       # ❌ 失败 (空名称)
```

### Range 验证

```
GET /api/RouteExamples/validation/range/25         # ✅ 成功 (18-100范围内)
GET /api/RouteExamples/validation/range/150        # ❌ 失败 (超出范围)
GET /api/RouteExamples/validation/range/10         # ❌ 失败 (小于最小值)
```

### StringLength 验证

```
GET /api/RouteExamples/validation/stringlength/john123     # ✅ 成功 (3-20字符)
GET /api/RouteExamples/validation/stringlength/ab          # ❌ 失败 (少于3字符)
GET /api/RouteExamples/validation/stringlength/verylongusernamethatexceedslimit  # ❌ 失败 (超过20字符)
```

### RegularExpression 验证

```
GET /api/RouteExamples/validation/regex/user@example.com   # ✅ 成功
GET /api/RouteExamples/validation/regex/user.test@sub.domain.com  # ✅ 成功
GET /api/RouteExamples/validation/regex/invalid-email      # ❌ 失败 (格式错误)
GET /api/RouteExamples/validation/regex/user@              # ❌ 失败 (不完整)
```

### 组合验证

```
GET /api/RouteExamples/validation/combined/100/张三/user@test.com     # ✅ 成功
GET /api/RouteExamples/validation/combined/1500/张三/user@test.com    # ❌ 失败 (ID超范围)
GET /api/RouteExamples/validation/combined/100/a/user@test.com        # ❌ 失败 (名字太短)
GET /api/RouteExamples/validation/combined/100/张三/invalid-email     # ❌ 失败 (邮箱格式)
```

### 自定义错误消息

```
GET /api/RouteExamples/validation/custom-message/85.5      # ✅ 成功
GET /api/RouteExamples/validation/custom-message/150.0     # ❌ 失败 (自定义错误消息)
GET /api/RouteExamples/validation/custom-message/-5.0      # ❌ 失败 (负数)
```

### 复杂模型验证

```
GET /api/RouteExamples/validation/model/123/validuser      # ✅ 成功
GET /api/RouteExamples/validation/model/0/validuser        # ❌ 失败 (用户ID无效)
GET /api/RouteExamples/validation/model/123/ab             # ❌ 失败 (用户名太短)
```

## 路由约束类型总结

### 数值约束

- `:int` - 整数
- `:decimal` - 小数
- `:double` - 双精度
- `:float` - 单精度
- `:long` - 长整数

### 范围约束

- `:min(value)` - 最小值
- `:max(value)` - 最大值
- `:range(min,max)` - 范围

### 字符串约束

- `:alpha` - 只允许字母
- `:alphanumeric` - 字母和数字
- `:length(value)` - 固定长度
- `:minlength(value)` - 最小长度
- `:maxlength(value)` - 最大长度

### 格式约束

- `:datetime` - 日期时间
- `:guid` - GUID 格式
- `:regex(pattern)` - 正则表达式

### 特殊约束

- `:required` - 必需参数
- `:file` - 文件名格式
- `:nonfile` - 非文件名格式
