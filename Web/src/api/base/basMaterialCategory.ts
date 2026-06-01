import {useBaseApi} from '/@/api/base';

// 物料分类接口服务
export const useBasMaterialCategoryApi = () => {
	const baseApi = useBaseApi("basMaterialCategory");
	return {
		// 分页查询物料分类
		page: baseApi.page,
		// 查看物料分类详细
		detail: baseApi.detail,
		// 新增物料分类
		add: baseApi.add,
		// 更新物料分类
		update: baseApi.update,
		// 删除物料分类
		delete: baseApi.delete,
		// 批量删除物料分类
		batchDelete: baseApi.batchDelete,
		// 导出物料分类数据
		exportData: baseApi.exportData,
		// 导入物料分类数据
		importData: baseApi.importData,
		// 下载物料分类数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
	}
}

// 物料分类实体
export interface BasMaterialCategory {
	// 主键Id
	id: number;
	// 分类编码
	categoryCode?: string;
	// 分类名称
	categoryName?: string;
	// 父分类ID
	parentId: number;
	// 排序
	sortOrder?: number;
	// 是否启用
	isEnabled?: number;
	// 备注
	remark: string;
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