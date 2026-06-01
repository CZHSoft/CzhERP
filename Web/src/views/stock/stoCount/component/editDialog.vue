<script lang="ts" name="stoCount" setup>
import { ref, reactive, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { formatDate } from '/@/utils/formatTime';
import { useStoCountApi } from '/@/api/stock/stoCount';
import { getWarehouseList } from '/@/api/stock/stoWarehouse';

const emit = defineEmits(["reloadTable"]);
const stoCountApi = useStoCountApi();
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
  { label: '盘点中', value: 'Counting' },
  { label: '已完成', value: 'Completed' }
];

// 表单验证规则
const rules = ref<FormRules>({
  countNo: [{required: true, message: '盘点单号由系统自动生成！', trigger: 'blur',},],
  warehouseName: [{required: true, message: '请选择仓库！', trigger: 'change',},],
  countDate: [{required: true, message: '请选择盘点日期！', trigger: 'change',},],
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

// 仓库选择变更
const handleWarehouseChange = (item: any) => {
	if (item) {
		state.ruleForm.warehouseId = item.id;
		state.ruleForm.warehouseCode = item.warehouseCode;
		state.ruleForm.warehouseName = item.warehouseName;
	} else {
		state.ruleForm.warehouseId = '';
		state.ruleForm.warehouseCode = '';
		state.ruleForm.warehouseName = '';
	}
};

// 打开弹窗
const openDialog = async (row: any, title: string) => {
	state.title = title;
	row = row ?? {  };
	if (row.id) {
		state.ruleForm = await stoCountApi.detail(row.id).then(res => res.data.result);
	} else {
		state.ruleForm = JSON.parse(JSON.stringify(row));
		// 新增时自动获取盘点单号
		const codeRes = await stoCountApi.getNextCode();
		state.ruleForm.countNo = codeRes.data.result;
		// 默认状态为草稿
		state.ruleForm.status = 'Draft';
		// 默认盘点日期为今天
		state.ruleForm.countDate = formatDate(new Date());
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
			await stoCountApi[state.ruleForm.id ? 'update' : 'add'](values);
			closeDialog();
		} else {
			ElMessage({
				message: `表单有${Object.keys(fields).length}处验证失败，请修改后再提交`,
				type: "error",
			});
		}
	});
};

defineExpose({ openDialog });
</script>
<template>
	<div class="stoCount-container">
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
						<el-form-item label="盘点单号" prop="countNo">
							<el-input v-model="state.ruleForm.countNo" placeholder="系统自动生成" :disabled="!state.ruleForm.id" maxlength="50" show-word-limit />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="盘点日期" prop="countDate">
							<el-date-picker v-model="state.ruleForm.countDate" type="date" placeholder="盘点日期" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="仓库" prop="warehouseName">
							<el-select 
								v-model="state.ruleForm.warehouseName" 
								placeholder="请选择仓库" 
								clearable
								@change="(val) => handleWarehouseChange(state.dropdownData.warehouseList.find(w => w.warehouseName === val))">
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
						<el-form-item label="差异数量" prop="diffQuantity">
							<el-input-number v-model="state.ruleForm.diffQuantity" placeholder="自动计算" :disabled="true" clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="差异金额" prop="diffAmount">
							<el-input-number v-model="state.ruleForm.diffAmount" placeholder="自动计算" :disabled="true" clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" class="mb20" >
						<el-form-item label="备注" prop="remark">
							<el-input v-model="state.ruleForm.remark" placeholder="请输入备注" maxlength="500" show-word-limit clearable />
						</el-form-item>
					</el-col>
					<!-- 隐藏字段 -->
					<el-form-item v-show="false" prop="warehouseId">
						<el-input v-model="state.ruleForm.warehouseId" />
					</el-form-item>
					<el-form-item v-show="false" prop="warehouseCode">
						<el-input v-model="state.ruleForm.warehouseCode" />
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