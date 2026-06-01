import {useBaseApi} from '/@/api/base';
import request from '/@/utils/request';

// 报价单明细接口服务
export const useSalQuoteItemApi = () => {
	const baseApi = useBaseApi("salQuoteItem");
	return {
		// 分页查询报价单明细
		page: baseApi.page,
		// 查看报价单明细详细
		detail: baseApi.detail,
		// 新增报价单明细
		add: baseApi.add,
		// 更新报价单明细
		update: baseApi.update,
		// 删除报价单明细
		delete: baseApi.delete,
		// 批量删除报价单明细
		batchDelete: baseApi.batchDelete,
		// 导出报价单明细数据
		exportData: baseApi.exportData,
		// 导入报价单明细数据
		importData: baseApi.importData,
		// 下载报价单明细数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
		// 获取报价单选择列表
		getQuoteList: (params: any) => {
			return request({
				url: "/api/salQuoteItem/QuoteList",
				method: "get",
				params,
			});
		},
		// 获取物料选择列表
		getMaterialList: (params: any) => {
			return request({
				url: "/api/salQuoteItem/MaterialList",
				method: "get",
				params,
			});
		},
		// 获取物料详情
		getMaterialDetail: (params: any) => {
			return request({
				url: "/api/salQuoteItem/MaterialDetail",
				method: "get",
				params,
			});
		},
	}
}

// 报价单明细实体
export interface SalQuoteItem {
	// 主键Id
	id: number;
	// 报价单ID
	quoteId?: number;
	// 报价单号
	quoteNo?: string;
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
	// 数量
	quantity?: number;
	// 单价
	unitPrice?: number;
	// 税率
	taxRate?: number;
	// 税额
	taxAmount?: number;
	// 金额
	amount?: number;
	// 折扣
	discount?: number;
	// 排序
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