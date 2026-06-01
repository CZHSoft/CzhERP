import {useBaseApi} from '/@/api/base';

// 采购发票表接口服务
export const usePurInvoiceApi = () => {
	const baseApi = useBaseApi("purInvoice");
	return {
		// 分页查询采购发票表
		page: baseApi.page,
		// 查看采购发票表详细
		detail: baseApi.detail,
		// 新增采购发票表
		add: baseApi.add,
		// 更新采购发票表
		update: baseApi.update,
		// 删除采购发票表
		delete: baseApi.delete,
		// 批量删除采购发票表
		batchDelete: baseApi.batchDelete,
		// 导出采购发票表数据
		exportData: baseApi.exportData,
		// 导入采购发票表数据
		importData: baseApi.importData,
		// 下载采购发票表数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
	}
}

// 采购发票表实体
export interface PurInvoice {
	// 主键Id
	id: number;
	// 发票号码
	invoiceNo?: string;
	// 关联订单ID
	orderId: number;
	// 关联入库单ID
	inboundId: number;
	// 供应商ID
	supplierId?: number;
	// 供应商名称
	supplierName?: string;
	// 发票类型(1增值税专票/2增值税普票/3电子发票)
	invoiceType?: number;
	// 开票日期
	invoiceDate?: string;
	// 不含税金额
	amount?: number;
	// 税率
	taxRate?: number;
	// 税额
	taxAmount?: number;
	// 价税合计
	grandTotal?: number;
	// 状态(0待审核/1已审核/2已核销/3已作废)
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