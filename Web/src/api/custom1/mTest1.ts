import {useBaseApi} from '/@/api/base';

// 自定义测试接口服务
export const useMTest1Api = () => {
	const baseApi = useBaseApi("mTest1");
	return {
		// 分页查询自定义测试
		page: baseApi.page,
		// 查看自定义测试详细
		detail: baseApi.detail,
		// 新增自定义测试
		add: baseApi.add,
		// 更新自定义测试
		update: baseApi.update,
		// 删除自定义测试
		delete: baseApi.delete,
		// 批量删除自定义测试
		batchDelete: baseApi.batchDelete,
		// 导出自定义测试数据
		exportData: baseApi.exportData,
		// 导入自定义测试数据
		importData: baseApi.importData,
		// 下载自定义测试数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
	}
}

// 自定义测试实体
export interface MTest1 {
	// 主键Id
	id: number;
	// 
	test1: string;
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