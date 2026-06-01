import {useBaseApi} from '/@/api/base';
import request from '/@/utils/request';

// 物料档案接口服务
export const useBasMaterialApi = () => {
	const baseApi = useBaseApi("basMaterial");
	return {
		// 分页查询物料档案
		page: baseApi.page,
		// 查看物料档案详细
		detail: baseApi.detail,
		// 新增物料档案
		add: baseApi.add,
		// 更新物料档案
		update: baseApi.update,
		// 删除物料档案
		delete: baseApi.delete,
		// 批量删除物料档案
		batchDelete: baseApi.batchDelete,
		// 导出物料档案数据
		exportData: baseApi.exportData,
		// 导入物料档案数据
		importData: baseApi.importData,
		// 下载物料档案数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
	}
}

// 获取物料列表接口（用于下拉选择）
export const getMaterialList = () => {
	return request.get<any[]>("/api/basMaterial/List");
}

// 物料档案实体
export interface BasMaterial {
	// 主键Id
	id: number;
	// 物料编码
	materialCode?: string;
	// 物料名称
	materialName?: string;
	// 规格型号
	spec: string;
	// 单位
	unit?: string;
	// 物料分类ID
	categoryId: number;
	// 物料分类编码
	categoryCode: string;
	// 物料分类名称
	categoryName: string;
	// 品牌
	brand: string;
	// 型号
	model: string;
	// 最低库存
	minStock?: number;
	// 最高库存
	maxStock?: number;
	// 成本价
	costPrice?: number;
	// 销售价
	salePrice?: number;
	// 税率
	taxRate?: number;
	// 是否启用
	isEnabled?: number;
	// 是否批次管理
	isBatchManage?: number;
	// 是否效期管理
	isExpiryManage?: number;
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