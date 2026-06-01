<script lang="ts" name="stoTransfer" setup>
import { ref, reactive, onMounted, watch } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { formatDate } from '/@/utils/formatTime';
import { useStoTransferApi } from '/@/api/stock/stoTransfer';
import { getWarehouseList } from '/@/api/stock/stoWarehouse';
//父级传递来的函数，用于回调
const emit = defineEmits(["reloadTable"]);
const stoTransferApi = useStoTransferApi();
const ruleFormRef = ref();

const state = reactive({
	title: '',
	loading: false,
	showDialog: false,
	ruleForm: {} as any,
	stores: {},
	dropdownData: {
		warehouseList: [] as any[],
	},
});

// 状态选项
const statusOptions = [
  { label: '草稿', value: 'Draft' },
  { label: '已审批', value: 'Approved' },
  { label: '已完成', value: 'Completed' },
  { label: '已取消', value: 'Cancelled' }
];

// 自行添加其他规则
const rules = ref<FormRules>({
  transferNo: [{required: true, message: '调拨单号由系统自动生成！', trigger: 'blur',},],
  fromWarehouseName: [{required: true, message: '请选择转出仓库！', trigger: 'change',},],
  toWarehouseName: [{required: true, message: '请选择转入仓库！', trigger: 'change',},],
  transferDate: [{required: true, message: '请选择调拨日期！', trigger: 'change',},],
});

// 页面加载时
onMounted(async () => {
	await loadWarehouseList();
});

// 加载仓库列表
const loadWarehouseList = async () => {
	const res = await getWarehouseList();
	state.dropdownData.warehouseList = res.data.result.items || [];
};

// 转出仓库选择变更
const handleFromWarehouseChange = (item: any) => {
	if (item) {
		state.ruleForm.fromWarehouseId = item.id;
		state.ruleForm.fromWarehouseCode = item.warehouseCode;
	} else {
		state.ruleForm.fromWarehouseId = '';
		state.ruleForm.fromWarehouseCode = '';
	}
};

// 转入仓库选择变更
const handleToWarehouseChange = (item: any) => {
	if (item) {
		state.ruleForm.toWarehouseId = item.id;
		state.ruleForm.toWarehouseCode = item.warehouseCode;
	} else {
		state.ruleForm.toWarehouseId = '';
		state.ruleForm.toWarehouseCode = '';
	}
};

// 打开弹窗
const openDialog = async (row: any, title: string) => {
	state.title = title;
	row = row ?? {  };
	if (row.id) {
		state.ruleForm = await stoTransferApi.detail(row.id).then(res => res.data.result);
	} else {
		state.ruleForm = JSON.parse(JSON.stringify(row));
		// 新增时自动获取调拨单号
		const codeRes = await stoTransferApi.getNextCode();
		state.ruleForm.transferNo = codeRes.data.result;
		// 默认状态为草稿
		state.ruleForm.status = 'Draft';
		// 默认调拨日期为今天
		state.ruleForm.transferDate = formatDate(new Date());
	}
	state.showDialog = true;
};

// 关闭弹窗
const closeDialog = () => {
	emit("reloadTable");
	state.showDialog = false;
};

// 提交
const submit = async () => {
	ruleFormRef.value.validate(async (isValid: boolean, fields?: any) => {
		if (isValid) {
			let values = state.ruleForm;
			await stoTransferApi[state.ruleForm.id ? 'update' : 'add'](values);
			closeDialog();
		} else {
			ElMessage({
				message: `表单有${Object.keys(fields).length}处验证失败，请修改后再提交`,
				type: "error",
			});
		}
	});
};

//将属性或者函数暴露给父组件
defineExpose({ openDialog });
</script>
<template>
	<div class="stoTransfer-container">
		<el-dialog v-model="state.showDialog" :width="800" draggable :close-on-click-modal="false">
			<template #header>
				<div style="color: #fff">
					<span>{{ state.title }}</span>
				</div>
			</template>
			<el-form :model="state.ruleForm" ref="ruleFormRef" label-width="auto" :rules="rules">
				<el-row :gutter="35">
					<el-form-item v-show="false">
						<el-input v-model="state.ruleForm.id" />
					</el-form-item>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="调拨单号" prop="transferNo">
							<el-input v-model="state.ruleForm.transferNo" placeholder="系统自动生成" :disabled="!state.ruleForm.id" maxlength="50" show-word-limit />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="调拨日期" prop="transferDate">
							<el-date-picker v-model="state.ruleForm.transferDate" type="date" placeholder="调拨日期" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="转出仓库" prop="fromWarehouseName">
							<el-select 
								v-model="state.ruleForm.fromWarehouseName" 
								placeholder="请选择转出仓库" 
								clearable
								@change="(val) => handleFromWarehouseChange(state.dropdownData.warehouseList.find(w => w.warehouseName === val))">
								<el-option 
									v-for="item in state.dropdownData.warehouseList" 
									:key="item.id" 
									:label="item.warehouseName" 
									:value="item.warehouseName" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="转入仓库" prop="toWarehouseName">
							<el-select 
								v-model="state.ruleForm.toWarehouseName" 
								placeholder="请选择转入仓库" 
								clearable
								@change="(val) => handleToWarehouseChange(state.dropdownData.warehouseList.find(w => w.warehouseName === val))">
								<el-option 
									v-for="item in state.dropdownData.warehouseList" 
									:key="item.id" 
									:label="item.warehouseName" 
									:value="item.warehouseName" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="状态" prop="status">
							<el-select v-model="state.ruleForm.status" placeholder="请选择状态" :disabled="!state.ruleForm.id">
								<el-option v-for="option in statusOptions" :key="option.value" :label="option.label" :value="option.value" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="备注" prop="remark">
							<el-input v-model="state.ruleForm.remark" placeholder="请输入备注" maxlength="500" show-word-limit clearable />
						</el-form-item>
					</el-col>
					<!-- 隐藏字段 -->
					<el-form-item v-show="false" prop="fromWarehouseId">
						<el-input v-model="state.ruleForm.fromWarehouseId" />
					</el-form-item>
					<el-form-item v-show="false" prop="fromWarehouseCode">
						<el-input v-model="state.ruleForm.fromWarehouseCode" />
					</el-form-item>
					<el-form-item v-show="false" prop="toWarehouseId">
						<el-input v-model="state.ruleForm.toWarehouseId" />
					</el-form-item>
					<el-form-item v-show="false" prop="toWarehouseCode">
						<el-input v-model="state.ruleForm.toWarehouseCode" />
					</el-form-item>
					<el-form-item v-show="false" prop="totalQuantity">
						<el-input-number v-model="state.ruleForm.totalQuantity" />
					</el-form-item>
				</el-row>
			</el-form>
			<template #footer>
				<span class="dialog-footer">
					<el-button @click="() => state.showDialog = false">取 消</el-button>
					<el-button @click="submit" type="primary" v-reclick="1000">确 定</el-button>
				</span>
			</template>
		</el-dialog>
	</div>
</template>
<style lang="scss" scoped>
:deep(.el-select), :deep(.el-input-number) {
  width: 100%;
}
</style>