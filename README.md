# CzhERP 企业资源管理系统


**基于 Admin.NET 框架的精简版 ERP 系统**


---

## 📖 项目简介

CzhERP 是一款基于 Admin.NET 框架开发的精简版企业资源管理系统，采用前后端分离架构，实现对企业核心业务流程的数字化管理。系统围绕**销售**、**采购**、**库存**、**财务**四大业务主线，构建了完整的企业运营闭环，覆盖从客户询价到财务记账的全链路业务场景。

本项目在 Admin.NET 框架基础上进行二次开发，整合了领域事件驱动架构、批量数据处理优化、业务流程自动化等企业级应用特性，为中小企业提供轻量化、可扩展的 ERP 解决方案。

### 🎯 项目特点

- **前后端分离架构**：后端基于 .NET 8 + Furion 框架，前端采用 Vue3 + TypeScript
- **领域事件驱动**：引入事件总线实现业务解耦，支持异步处理和业务流程自动化
- **批量数据处理**：优化 Excel 导入导出，支持万级记录批量操作
- **多业务模块集成**：销售、采购、库存、财务模块无缝衔接
- **规范开发体系**：遵循 RESTful API 规范，支持 Swagger 在线文档

---

## 🏗️ 技术架构

### 后端技术栈

| 技术选型 | 版本 | 说明 |
|---------|------|------|
| **.NET** | 8.0 | 跨平台运行时 |
| **Furion** | 4.x | 核心应用框架 |
| **SqlSugar** | 5.x | ORM 数据库访问框架 |
| **MySQL** | 8.0 | 关系型数据库 |

### 前端技术栈

| 技术选型 | 版本 | 说明 |
|---------|------|------|
| **Vue** | 3.4 | 渐进式前端框架 |
| **TypeScript** | 5.0 | 类型化 JavaScript |
| **Vite** | 5.x | 下一代前端构建工具 |
| **Element Plus** | 2.x | Vue3 组件库 |
| **Pinia** | 2.x | 状态管理 |
| **Axios** | 1.x | HTTP 请求库 |

### 基础设施

| 组件 | 说明 |
|------|------|
| Redis | 缓存、事件队列 |
| Swagger/Knife4j | API 接口文档 |
| JWT | 无状态身份认证 |

---

## 📦 核心功能模块

### 销售管理 (Sales)

| 功能 | 说明 |
|------|------|
| 客户管理 | 客户档案、联系人、信用额度 |
| 销售报价 | 报价单创建、审批、有效期管理 |
| 销售订单 | 订单创建、信用校验、订单跟踪 |
| 销售出库 | 出库确认、库存扣减、物流跟踪 |
| 销售退货 | 退货受理、退货入库、退款处理 |

### 采购管理 (Purchase)

| 功能 | 说明 |
|------|------|
| 供应商管理 | 供应商档案、评级、资质管理 |
| 采购申请 | 采购需求申请、多级审批 |
| 采购订单 | 订单创建、跟踪、变更管理 |
| 采购入库 | 入库确认、质检联动、库存更新 |
| 采购退货 | 退货申请、退货出库 |

### 库存管理 (Stock)

| 功能 | 说明 |
|------|------|
| 仓库管理 | 多仓库、库区、库位设置 |
| 入库管理 | 采购入库、其他入库 |
| 出库管理 | 销售出库、其他出库 |
| 库存查询 | 实时库存、批次追踪 |
| 库存盘点 | 盘点计划、差异分析、调整处理 |
| 库存预警 | 安全库存、呆滞物料预警 |

### 财务管理 (Finance)

| 功能 | 说明 |
|------|------|
| 会计凭证 | 凭证录入、审核、过账 |
| 应收应付 | 往来账管理、核销处理 |
| 收付款 | 收款付款、审批流程 |
| 账簿管理 | 总账、明细账、科目余额 |
| 财务报表 | 资产负债表、利润表 |
| 税务管理 | 税率配置、进项销项税 |
| 资金管理 | 银行账户、资金流水 |
| 预算管理 | 预算编制、执行监控 |

---

## 🔧 环境配置要求

### 运行环境

- **操作系统**：Windows 10+ / Windows Server 2019+ / Linux (Ubuntu 20.04+)
- **.NET SDK**：8.0 或更高版本
- **Node.js**：18.x 或更高版本
- **MySQL**：8.0 或更高版本
- **Redis**：6.0 或更高版本（可选，用于缓存和事件队列）

### 开发工具推荐

- **IDE**：Visual Studio 2022 / Rider / VS Code
- **数据库工具**：Navicat / DBeaver / MySQL Workbench
- **API 测试**：Postman / Apifox / Swagger UI
- **版本控制**：Git

---

## 🚀 安装部署

### 1. 克隆代码

```bash
git clone https://github.com/CZHSoft/CzhERP.git
cd CzhERP/Admin.NET
```

### 2. 配置数据库

创建 MySQL 数据库并执行初始化脚本：

```sql
CREATE DATABASE czh_erp DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
```

修改 `appsettings.json` 中的数据库连接字符串：

```json
{
  "ConnectionStrings": {
    "Default": "Server=localhost;Database=czh_erp;Uid=root;Pwd=your_password;Charset=utf8mb4;"
  }
}
```

### 3. 配置 Redis（可选）

```json
{
  "Redis": {
    "Default": {
      "ConnectionString": "localhost:6379,defaultDatabase=0,abortConnect=false"
    }
  }
}
```

### 4. 启动后端服务

```bash
cd Admin.NET/Admin.NET.Web.Entry
dotnet restore
dotnet run
```

服务默认运行在 `http://localhost:5000`，Swagger UI 访问地址：`http://localhost:5000/swagger`

### 5. 启动前端服务

```bash
cd Web
pnpm install
pnpm run dev
```

前端默认运行在 `http://localhost:5173`

### 6. Docker 部署（可选）

```bash
cd docker
docker-compose up -d
```

---

## 📖 使用指南

### 登录系统

1. 访问前端地址：`http://localhost:5173`
2. 使用默认账号登录：
   - 用户名：`superAdmin.NET`
   - 密码：`Admin.NET++010101`

### 基础数据初始化

首次使用建议按以下顺序初始化基础数据：

```
1. 系统管理 → 机构管理：维护组织架构
2. 系统管理 → 用户管理：创建业务用户
3. 系统管理 → 角色管理：配置角色权限
4. 基础数据 → 物料管理：维护物料档案
5. 基础数据 → 仓库管理：设置仓库信息
```

### 业务流程示例

#### 销售报价转订单流程

1. **创建报价单**：`销售管理 → 销售报价 → 新增报价单`
2. **填写报价明细**：添加产品、价格、数量
3. **提交审批**：点击审批按钮
4. **自动转单**：审批通过后自动生成销售订单

#### 采购入库流程

1. **创建采购订单**：`采购管理 → 采购订单 → 新增订单`
2. **供应商送货**：供应商送货到仓库
3. **创建入库单**：`库存管理 → 采购入库 → 新增入库单`
4. **质检确认**：关联质检记录
5. **入库确认**：完成库存更新

---

## 📚 API 文档

系统提供完整的 RESTful API 接口，访问 Swagger 文档：

```
http://localhost:5000/swagger
```

### 主要 API 模块

| 模块 | 路径前缀 | 说明 |
|------|----------|------|
| 销售 | `/api/sal` | 客户、报价、订单、出库 |
| 采购 | `/api/pur` | 供应商、申请、订单、入库 |
| 库存 | `/api/sto` | 仓库、库位、库存、盘点 |
| 财务 | `/api/fin` | 凭证、应收应付、收付款 |

### 通用接口规范

| 操作 | HTTP 方法 | 路径格式 |
|------|-----------|----------|
| 分页查询 | POST | `/api/{模块}/{服务}/Page` |
| 新增 | POST | `/api/{模块}/{服务}/Add` |
| 更新 | POST | `/api/{模块}/{服务}/Update` |
| 删除 | POST | `/api/{模块}/{服务}/Delete` |

---

## 📂 项目结构

```
CzhERP/
├── Admin.NET/                          # 后端项目
│   ├── Admin.NET.sln                   # 解决方案文件
│   ├── Admin.NET/                      # 框架核心层
│   │   ├── Admin.NET.Core/             # 核心基础设施
│   │   ├── Admin.NET.Application/      # 应用服务层
│   │   ├── Admin.NET.Web.Core/         # Web 配置层
│   │   └── Admin.NET.Web.Entry/        # 入口启动层
│   ├── CzhERP.Application/             # 业务应用层
│   │   ├── Entity/                     # 实体定义
│   │   ├── Service/                    # 业务服务
│   │   │   ├── SalQuote/              # 销售报价
│   │   │   ├── SalOrder/              # 销售订单
│   │   │   ├── PurSupplier/          # 供应商管理
│   │   │   ├── StoInventory/         # 库存管理
│   │   │   └── FinVoucher/           # 财务凭证
│   │   └── EventSubscribers/          # 事件订阅者
│   └── Web/                           # 前端项目
│       ├── src/
│       │   ├── api/                   # API 接口定义
│       │   ├── views/                # 页面组件
│       │   ├── components/            # 公共组件
│       │   ├── stores/                # 状态管理
│       │   └── utils/                # 工具函数
│       └── package.json
├── doc/                               # 项目文档
│   ├── 技术文档/                      # 开发文档
│   └── README.md                      # 项目说明
└── docker/                           # Docker 配置
```

---

## 🔌 事件驱动架构

系统采用领域事件驱动设计，业务模块间通过事件总线通信：

```csharp
// 发布事件
await _eventPublisher.PublishAsync(EventBusConst.SalQuoteApproved, 
    new SalQuoteApprovedEvent { QuoteId = quote.Id, ... });

// 订阅处理
public class SalQuoteEventSubscriber : IEventSubscriber
{
    [EventSubscribe(EventBusConst.SalQuoteApproved)]
    public async Task HandleQuoteApproved(EventHandlerExecutingContext context)
    {
        // 业务处理逻辑
    }
}
```

### 预定义事件

| 事件名称 | 触发场景 | 处理逻辑 |
|----------|----------|----------|
| `SalQuoteApproved` | 报价单审批通过 | 自动生成销售订单 |
| `SalOrderApproved` | 订单审批通过 | 更新订单状态 |
| `FinPaymentApproved` | 付款审批通过 | 更新往来账 |
| `FinReceiptApproved` | 收款审批通过 | 核销应收账款 |

---

## 🤝 贡献指南

欢迎提交 Issue 和 Pull Request！

### 提交 Issue

- 描述清楚问题或建议
- 提供复现步骤（如适用）
- 标注相关模块和版本

### 提交代码

1. Fork 本仓库
2. 创建特性分支：`git checkout -b feature/your-feature`
3. 提交更改：`git commit -m 'Add: your feature'`
4. 推送分支：`git push origin feature/your-feature`
5. 创建 Pull Request

### 开发规范

- 遵循 C# 编码规范（命名、注释）
- 前端代码使用 ESLint 检查
- 提交前运行测试确保功能正常

---

## 📄 许可证

本项目基于 **MIT** 许可证开源，详见 [LICENSE](LICENSE) 文件。

---

## 📞 联系方式

- **GitHub Issues**: [https://github.com/CZHSoft/CzhERP/issues](https://github.com/CZHSoft/CzhERP/issues)
- **项目主页**: [https://github.com/CZHSoft/CzhERP](https://github.com/CZHSoft/CzhERP)

---


**如果对您有帮助，请点击 ⭐ Star 支持一下！**


