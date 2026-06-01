import {useBaseApi} from '/@/api/base';
import request from '/@/utils/request';

// 采购申请单主表接口服务
export const usePurRequisitionApi = () => {
	const baseApi = useBaseApi("purRequisition");
	return {
		// 分页查询采购申请单主表
		page: baseApi.page,
		// 查看采购申请单主表详细
		detail: baseApi.detail,
		// 新增采购申请单主表
		add: baseApi.add,
		// 更新采购申请单主表
		update: baseApi.update,
		// 删除采购申请单主表
		delete: baseApi.delete,
		// 批量删除采购申请单主表
		batchDelete: baseApi.batchDelete,
		// 导出采购申请单主表数据
		exportData: baseApi.exportData,
		// 导入采购申请单主表数据
		importData: baseApi.importData,
		// 下载采购申请单主表数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
		// 获取下一个申请单号
		getNextCode: () => {
			return request({
				url: '/api/purRequisition/nextCode',
				method: 'get',
			});
		},
		// 审核采购申请单
		approve: (data: { id: number; approvalRemark?: string; approvalUserId?: number }) => {
			return request({
				url: '/api/purRequisition/approve',
				method: 'post',
				data,
			});
		},
		// 拒绝采购申请单
		reject: (data: { id: number; rejectReason: string; approvalUserId?: number }) => {
			return request({
				url: '/api/purRequisition/reject',
				method: 'post',
				data,
			});
		},
		// 计算采购申请单明细汇总
		calculateSummary: (params: { requisitionId: number }) => {
			return request({
				url: '/api/purRequisition/calculateSummary',
				method: 'get',
				params,
			});
		},
		// 获取采购申请单列表（用于下拉选择）
		list: (params?: any) => {
			return request({
				url: '/api/purRequisition/list',
				method: 'get',
				params,
			});
		},
	}
}

// 采购申请单主表实体
export interface PurRequisition {
	// 主键Id
	id: number;
	// 申请单号
	requisitionNo?: string;
	// 申请部门
	departmentId?: number;
	// 申请人
	applicantId?: number;
	// 申请日期
	applyDate?: string;
	// 期望到货日期
	expectedDate: string;
	// 总金额
	totalAmount?: number;
	// 状态(0草稿/1提交/2审批中/3通过/4拒绝)
	status?: number;
	// 用途说明
	purpose: string;
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