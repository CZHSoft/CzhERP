import {useBaseApi} from '/@/api/base';

// 供应商主表接口服务
export const usePurSupplierApi = () => {
	const baseApi = useBaseApi("purSupplier");
	return {
		// 分页查询供应商主表
		page: baseApi.page,
		// 查看供应商主表详细
		detail: baseApi.detail,
		// 新增供应商主表
		add: baseApi.add,
		// 更新供应商主表
		update: baseApi.update,
		// 删除供应商主表
		delete: baseApi.delete,
		// 批量删除供应商主表
		batchDelete: baseApi.batchDelete,
		// 导出供应商主表数据
		exportData: baseApi.exportData,
		// 导入供应商主表数据
		importData: baseApi.importData,
		// 下载供应商主表数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
	}
}

// 供应商主表实体
export interface PurSupplier {
	// 主键Id
	id: number;
	// 供应商编码
	supplierCode?: string;
	// 供应商名称
	supplierName?: string;
	// 简称
	shortName: string;
	// 分类ID
	categoryId: number;
	// 联系人
	contactName: string;
	// 联系电话
	phone: string;
	// 手机
	mobile: string;
	// 邮箱
	email: string;
	// 地址
	address: string;
	// 开户银行
	bankName: string;
	// 银行账号
	bankAccount: string;
	// 税号
	taxNo: string;
	// 信用等级(1-5)
	creditRating?: number;
	// 状态(0禁用/1启用)
	status?: number;
	// 是否黑名单
	isBlacklist?: boolean;
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