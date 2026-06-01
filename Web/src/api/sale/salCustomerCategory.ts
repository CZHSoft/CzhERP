import {useBaseApi} from '/@/api/base';

// 客户分类接口服务
export const useSalCustomerCategoryApi = () => {
	const baseApi = useBaseApi("salCustomerCategory");
	return {
		// 分页查询客户分类
		page: baseApi.page,
		// 查看客户分类详细
		detail: baseApi.detail,
		// 新增客户分类
		add: baseApi.add,
		// 更新客户分类
		update: baseApi.update,
		// 删除客户分类
		delete: baseApi.delete,
		// 批量删除客户分类
		batchDelete: baseApi.batchDelete,
		// 导出客户分类数据
		exportData: baseApi.exportData,
		// 导入客户分类数据
		importData: baseApi.importData,
		// 下载客户分类数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
	}
}

// 客户分类实体
export interface SalCustomerCategory {
	// 主键Id
	id: number;
	// 分类编码
	categoryCode?: string;
	// 分类名称
	categoryName?: string;
	// 上级分类ID
	parentId: number;
	// 排序
	sortOrder?: number;
	// 是否启用
	isEnabled?: boolean;
	// 创建时间
	createTime: string;
	// 更新时间
	updateTime: string;
	// 创建者Id
	createUserId: number;
	// 创建者姓名
	createUserName: string;
	// 修改者Id
	updateUserId: number;
	// 修改者姓名
	updateUserName: string;
}