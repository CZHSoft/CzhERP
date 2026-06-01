import {useBaseApi} from '/@/api/base';
import request from '/@/utils/request';

// 仓库档案接口服务
export const useStoWarehouseApi = () => {
	const baseApi = useBaseApi("stoWarehouse");
	return {
		// 分页查询仓库档案
		page: baseApi.page,
		// 查看仓库档案详细
		detail: baseApi.detail,
		// 新增仓库档案
		add: baseApi.add,
		// 更新仓库档案
		update: baseApi.update,
		// 删除仓库档案
		delete: baseApi.delete,
		// 批量删除仓库档案
		batchDelete: baseApi.batchDelete,
		// 导出仓库档案数据
		exportData: baseApi.exportData,
		// 导入仓库档案数据
		importData: baseApi.importData,
		// 下载仓库档案数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
		// 获取下一个仓库编码
		getNextCode: () => request.get<string>("/api/stoWarehouse/NextCode"),
	}
}

// 获取仓库列表（用于下拉选择）
export const getWarehouseList = (params?: any) => {
	return request({
		url: "/api/stoWarehouse/page",
		method: "POST",
		data: {
			page: 1,
			pageSize: 100,
			isEnabled: 1,
			...params,
		},
	});
};

// 仓库档案实体
export interface StoWarehouse {
	// 主键Id
	id: number;
	// 仓库编码
	warehouseCode?: string;
	// 仓库名称
	warehouseName?: string;
	// 仓库类型(Normal普通仓/Bonded保税仓/Return退货仓)
	warehouseType?: string;
	// 仓库地址
	address: string;
	// 省份
	province: string;
	// 城市
	city: string;
	// 仓库负责人
	contactName: string;
	// 联系电话
	contactPhone: string;
	// 仓库容量
	capacity?: number;
	// 是否启用
	isEnabled?: number;
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
