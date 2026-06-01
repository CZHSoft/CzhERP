import {useBaseApi} from '/@/api/base';

// 盘点单明细表接口服务
export const useStoCountItemApi = () => {
	const baseApi = useBaseApi("stoCountItem");
	return {
		// 分页查询盘点单明细表
		page: baseApi.page,
		// 查看盘点单明细表详细
		detail: baseApi.detail,
		// 新增盘点单明细表
		add: baseApi.add,
		// 更新盘点单明细表
		update: baseApi.update,
		// 删除盘点单明细表
		delete: baseApi.delete,
		// 批量删除盘点单明细表
		batchDelete: baseApi.batchDelete,
		// 导出盘点单明细表数据
		exportData: baseApi.exportData,
		// 导入盘点单明细表数据
		importData: baseApi.importData,
		// 下载盘点单明细表数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
	}
}

// 盘点单明细表实体
export interface StoCountItem {
	// 主键Id
	id: number;
	// 盘点单ID
	countId?: number;
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
	// 系统数量
	systemQuantity?: number;
	// 实际数量
	actualQuantity?: number;
	// 差异数量
	diffQuantity?: number;
	// 成本单价
	costPrice?: number;
	// 差异金额
	diffAmount?: number;
	// 批号
	batchNo: string;
	// 库位编码
	locationCode: string;
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