import {useBaseApi} from '/@/api/base';

// 税率配置表接口服务
export const useFinTaxRateApi = () => {
	const baseApi = useBaseApi("finTaxRate");
	return {
		// 分页查询税率配置表
		page: baseApi.page,
		// 查看税率配置表详细
		detail: baseApi.detail,
		// 新增税率配置表
		add: baseApi.add,
		// 更新税率配置表
		update: baseApi.update,
		// 删除税率配置表
		delete: baseApi.delete,
		// 批量删除税率配置表
		batchDelete: baseApi.batchDelete,
		// 导出税率配置表数据
		exportData: baseApi.exportData,
		// 导入税率配置表数据
		importData: baseApi.importData,
		// 下载税率配置表数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
	}
}

// 税率配置表实体
export interface FinTaxRate {
	// 主键Id
	id: number;
	// 税种编码
	taxCode?: string;
	// 税种名称
	taxName?: string;
	// 税种类型
	taxType: string;
	// 税率
	taxRateValue?: number;
	// 对应科目编码
	accountCode: string;
	// 生效日期
	effectiveDate?: string;
	// 失效日期
	expiryDate: string;
	// 是否启用
	isEnabled?: boolean;
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