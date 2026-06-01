
import {useBaseApi} from '/@/api/base';
import request from '/@/utils/request';

// 采购入库单主表接口服务
export const usePurInboundApi = () => {
	const baseApi = useBaseApi("purInbound");
	return {
		// 分页查询采购入库单主表
		page: baseApi.page,
		// 查看采购入库单主表详细
		detail: baseApi.detail,
		// 新增采购入库单主表
		add: baseApi.add,
		// 更新采购入库单主表
		update: baseApi.update,
		// 删除采购入库单主表
		delete: baseApi.delete,
		// 批量删除采购入库单主表
		batchDelete: baseApi.batchDelete,
		// 导出采购入库单主表数据
		exportData: baseApi.exportData,
		// 导入采购入库单主表数据
		importData: baseApi.importData,
		// 下载采购入库单主表数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
		// 获取下一个入库单号
		getNextCode: () => {
			return request({
				url: '/api/purInbound/nextCode',
				method: 'get',
			});
		},
		// 获取采购入库单列表（用于下拉选择）
		list: () => {
			return request({
				url: '/api/purInbound/list',
				method: 'get',
			});
		},
		// 根据采购订单创建入库单
		createFromPurOrder: (data: any) => {
			return request({
				url: '/api/purInbound/CreateFromPurOrder',
				method: 'post',
				data,
			});
		},
	}
}

// 采购入库单主表实体
export interface PurInbound {
	// 主键Id
	id: number;
	// 入库单号
	inboundNo?: string;
	// 采购订单ID
	orderId?: number;
	// 采购订单号
	orderNo?: string;
	// 供应商ID
	supplierId?: number;
	// 供应商名称
	supplierName?: string;
	// 仓库ID
	warehouseId?: number;
	// 仓库名称
	warehouseName?: string;
	// 入库日期
	inboundDate?: string;
	// 到货日期
	arrivalDate: string;
	// 总数量
	totalQty?: number;
	// 总金额
	totalAmount?: number;
	// 状态(0待质检/1质检中/2合格/3部分合格/4不合格)
	status?: number;
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
