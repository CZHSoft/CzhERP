import {useBaseApi} from '/@/api/base';

// 物流跟踪接口服务
export const useSalDeliveryApi = () => {
	const baseApi = useBaseApi("salDelivery");
	return {
		// 分页查询物流跟踪
		page: baseApi.page,
		// 查看物流跟踪详细
		detail: baseApi.detail,
		// 新增物流跟踪
		add: baseApi.add,
		// 更新物流跟踪
		update: baseApi.update,
		// 删除物流跟踪
		delete: baseApi.delete,
		// 批量删除物流跟踪
		batchDelete: baseApi.batchDelete,
		// 导出物流跟踪数据
		exportData: baseApi.exportData,
		// 导入物流跟踪数据
		importData: baseApi.importData,
		// 下载物流跟踪数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
	}
}

// 物流跟踪实体
export interface SalDelivery {
	// 主键Id
	id: number;
	// 出库单ID
	outboundId?: number;
	// 运单号
	trackingNo?: string;
	// 物流公司
	logisticsCompany: string;
	// 物流状态
	status: string;
	// 当前位置
	currentLocation: string;
	// 更新时间
	updateTime: string;
	// 备注
	remark: string;
	// 创建时间
	createTime: string;
	// 创建者Id
	createUserId: number;
	// 创建者姓名
	createUserName: string;
	// 修改者Id
	updateUserId: number;
	// 修改者姓名
	updateUserName: string;
}