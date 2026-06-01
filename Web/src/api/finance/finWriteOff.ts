import {useBaseApi} from '/@/api/base';

// 核销记录表接口服务
export const useFinWriteOffApi = () => {
	const baseApi = useBaseApi("finWriteOff");
	return {
		// 分页查询核销记录表
		page: baseApi.page,
		// 查看核销记录表详细
		detail: baseApi.detail,
		// 新增核销记录表
		add: baseApi.add,
		// 更新核销记录表
		update: baseApi.update,
		// 删除核销记录表
		delete: baseApi.delete,
		// 批量删除核销记录表
		batchDelete: baseApi.batchDelete,
		// 导出核销记录表数据
		exportData: baseApi.exportData,
		// 导入核销记录表数据
		importData: baseApi.importData,
		// 下载核销记录表数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
		// 获取核销类型字典
		getWriteOffTypes: () => baseApi.request({ url: "/api/finWriteOff/getWriteOffTypes" }),
		// 获取业务类型字典
		getBusinessTypes: () => baseApi.request({ url: "/api/finWriteOff/getBusinessTypes" }),
		// 获取核销状态字典
		getStatuses: () => baseApi.request({ url: "/api/finWriteOff/getStatuses" }),
		// 获取新核销单号
		getNewWriteOffNo: () => baseApi.request({ url: "/api/finWriteOff/getNewWriteOffNo" }),
	}
}

// 核销记录表实体
export interface FinWriteOff {
	// 主键Id
	id: number;
	// 核销单号
	writeOffNo?: string;
	// 核销类型
	writeOffType?: string;
	// 业务类型
	businessType?: string;
	// 关联单据ID
	businessId?: number;
	// 关联单据号
	businessNo?: string;
	// 核销金额
	writeOffAmount?: number;
	// 剩余金额
	remainAmount?: number;
	// 核销日期
	writeOffDate?: string;
	// 核销人ID
	writeOffUserId: number;
	// 核销人姓名
	writeOffUserName: string;
	// 状态
	status?: string;
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