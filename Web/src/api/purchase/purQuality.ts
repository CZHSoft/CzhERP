import {useBaseApi} from '/@/api/base';
import request from '/@/utils/request';

// 质检记录表接口服务
export const usePurQualityApi = () => {
	const baseApi = useBaseApi("purQuality");
	return {
		// 分页查询质检记录表
		page: baseApi.page,
		// 查看质检记录表详细
		detail: baseApi.detail,
		// 新增质检记录表
		add: baseApi.add,
		// 更新质检记录表
		update: baseApi.update,
		// 删除质检记录表
		delete: baseApi.delete,
		// 批量删除质检记录表
		batchDelete: baseApi.batchDelete,
		// 导出质检记录表数据
		exportData: baseApi.exportData,
		// 导入质检记录表数据
		importData: baseApi.importData,
		// 下载质检记录表数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
		// 获取下一个质检单号
		getNextCode: () => {
			return request({
				url: '/api/purQuality/nextCode',
				method: 'get',
			});
		},
		// 审核质检记录
		approve: (id: number) => {
			return request({
				url: '/api/purQuality/approve',
				method: 'post',
				data: { id },
			});
		},
	}
}

// 质检记录表实体
export interface PurQuality {
	// 主键Id
	id: number;
	// 质检单号
	qualityNo?: string;
	// 关联入库单ID
	inboundId?: number;
	// 入库单号(用于展示)
	inboundNo?: string;
	// 检验类型(1全检/2抽检)
	inspectionType?: number;
	// 抽样数量
	sampleQty: number;
	// 合格数量
	passQty?: number;
	// 不合格数量
	failQty?: number;
	// 检验结果(0待判定/1合格/2不合格/3让步接收)
	result?: number;
	// 检验员ID
	inspectorId?: number;
	// 检验日期
	inspectionDate?: string;
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
