import {useBaseApi} from '/@/api/base';

// 调整单明细表接口服务
export const useStoAdjustItemApi = () => {
	const baseApi = useBaseApi("stoAdjustItem");
	return {
		// 分页查询调整单明细表
		page: baseApi.page,
		// 查看调整单明细表详细
		detail: baseApi.detail,
		// 新增调整单明细表
		add: baseApi.add,
		// 更新调整单明细表
		update: baseApi.update,
		// 删除调整单明细表
		delete: baseApi.delete,
		// 批量删除调整单明细表
		batchDelete: baseApi.batchDelete,
		// 导出调整单明细表数据
		exportData: baseApi.exportData,
		// 导入调整单明细表数据
		importData: baseApi.importData,
		// 下载调整单明细表数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
	}
}

// 调整单明细表实体
export interface StoAdjustItem {
	// 主键Id
	id: number;
	// 调整单ID
	adjustId?: number;
	// 物料ID
	materialId?: number;
	// 物料编码
	materialCode?: string;
	// 物料名称
	materialName?: string;
	// 规格型号
	spec: string;
	// 单位
	unit?: string;
	// 调整数量(正数增加,负数减少)
	adjustQuantity?: number;
	// 成本单价
	costPrice?: number;
	// 调整金额
	adjustAmount?: number;
	// 库位编码
	locationCode: string;
	// 批号
	batchNo: string;
	// 排序号
	sortOrder?: number;
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