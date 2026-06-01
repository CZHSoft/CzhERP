import {useBaseApi} from '/@/api/base';

// 供应商分类表接口服务
export const usePurSupplierCategoryApi = () => {
	const baseApi = useBaseApi("purSupplierCategory");
	return {
		// 分页查询供应商分类表
		page: baseApi.page,
		// 查看供应商分类表详细
		detail: baseApi.detail,
		// 新增供应商分类表
		add: baseApi.add,
		// 更新供应商分类表
		update: baseApi.update,
		// 删除供应商分类表
		delete: baseApi.delete,
		// 批量删除供应商分类表
		batchDelete: baseApi.batchDelete,
		// 导出供应商分类表数据
		exportData: baseApi.exportData,
		// 导入供应商分类表数据
		importData: baseApi.importData,
		// 下载供应商分类表数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
	}
}

// 供应商分类表实体
export interface PurSupplierCategory {
	// 主键Id
	id: number;
	// 分类编码
	categoryCode?: string;
	// 分类名称
	categoryName?: string;
	// 上级分类ID
	parentId: number;
	// 层级(1-5)
	level?: number;
	// 排序
	sortOrder?: number;
	// 状态(0禁用/1启用)
	status?: number;
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