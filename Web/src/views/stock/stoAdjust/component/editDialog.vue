<script lang="ts" name="stoAdjust" setup>
import { ref, reactive, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { formatDate } from '/@/utils/formatTime';
import { useStoAdjustApi } from '/@/api/stock/stoAdjust';
import { getWarehouseList } from '/@/api/stock/stoWarehouse';

const emit = defineEmits(["reloadTable"]);
const stoAdjustApi = useStoAdjustApi();
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

const adjustTypeOptions = [
  { label: '调整', value: 'Adjust' },
  { label: '盘点差异', value: 'CountDiff' }
];

const statusOptions = [
  { label: '草稿', value: 'Draft' },
  { label: '已审核', value: 'Approved' },
  { label: '已完成', value: 'Completed' },
  { label: '已取消', value: 'Cancelled' }
];

const rules = ref<FormRules>({
  adjustNo: [{required: true, message: '调整单号由系统自动生成！', trigger: 'blur',}],
  adjustType: [{required: true, message: '请选择调整类型！', trigger: 'change',}],
  warehouseName: [{required: true, message: '请选择仓库！', trigger: 'change',}],
  adjustDate: [{required: true, message: '请选择调整日期！', trigger: 'change',}],
});

onMounted(async () => {
	await loadWarehouseList();
});

const loadWarehouseList = async () => {
	const res = await getWarehouseList();
	state.dropdownData.warehouseList = res.data.result.items || [];
};

const handleWarehouseChange = (item: any) => {
	if (item) {
		state.ruleForm.warehouseId = item.id;
		state.ruleForm.warehouseCode = item.warehouseCode;
	} else {
		state.ruleForm.warehouseId = '';
		state.ruleForm.warehouseCode = '';
	}
};

const openDialog = async (row: any, title: string) => {
	state.title = title;
	row = row ?? {  };
	if (row.id) {
		state.ruleForm = await stoAdjustApi.detail(row.id).then(res => res.data.result);
	} else {
		state.ruleForm = JSON.parse(JSON.stringify(row));
		const codeRes = await stoAdjustApi.getNextCode();
		state.ruleForm.adjustNo = codeRes.data.result;
		state.ruleForm.adjustType = 'Adjust';
		state.ruleForm.status = 'Draft';
		state.ruleForm.adjustDate = formatDate(new Date());
	}
	state.showDialog = true;
};

const closeDialog = () => {
	emit("reloadTable");
	state.showDialog = false;
};

const submit = async () => {
	ruleFormRef.value.validate(async (isValid: boolean, fields?: any) => {
		if (isValid) {
			let values = state.ruleForm;
			await stoAdjustApi[state.ruleForm.id ? 'update' : 'add'](values);
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
	<div class="stoAdjust-container">
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
						<el-form-item label="调整单号" prop="adjustNo">
							<el-input v-model="state.ruleForm.adjustNo" placeholder="系统自动生成" :disabled="true" maxlength="50" show-word-limit />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="调整类型" prop="adjustType">
							<el-select v-model="state.ruleForm.adjustType" placeholder="请选择调整类型" clearable>
								<el-option v-for="option in adjustTypeOptions" :key="option.value" :label="option.label" :value="option.value" />
							</el-select>
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
						<el-form-item label="调整日期" prop="adjustDate">
							<el-date-picker v-model="state.ruleForm.adjustDate" type="date" placeholder="调整日期" />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="状态" prop="status">
							<el-select v-model="state.ruleForm.status" placeholder="请选择状态" :disabled="true">
								<el-option v-for="option in statusOptions" :key="option.value" :label="option.label" :value="option.value" />
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20" >
						<el-form-item label="调整原因" prop="adjustReason">
							<el-input v-model="state.ruleForm.adjustReason" placeholder="请输入调整原因" maxlength="200" show-word-limit clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20" >
						<el-form-item label="备注" prop="remark">
							<el-input v-model="state.ruleForm.remark" placeholder="请输入备注" maxlength="500" show-word-limit clearable />
						</el-form-item>
					</el-col>
					<el-form-item v-show="false" prop="warehouseId">
						<el-input v-model="state.ruleForm.warehouseId" />
					</el-form-item>
					<el-form-item v-show="false" prop="warehouseCode">
						<el-input v-model="state.ruleForm.warehouseCode" />
					</el-form-item>
					<el-form-item v-show="false" prop="totalQuantity">
						<el-input-number v-model="state.ruleForm.totalQuantity" />
					</el-form-item>
					<el-form-item v-show="false" prop="totalAmount">
						<el-input-number v-model="state.ruleForm.totalAmount" />
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