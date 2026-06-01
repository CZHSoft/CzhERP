<script lang="ts" name="finBudget" setup>
import { ref, reactive, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { formatDate } from '/@/utils/formatTime';
import { useFinBudgetApi } from '/@/api/finance/finBudget';

const emit = defineEmits(["reloadTable"]);
const finBudgetApi = useFinBudgetApi();
const ruleFormRef = ref();

const state = reactive({
	title: '',
	loading: false,
	showDialog: false,
	ruleForm: {} as any,
	stores: {},
	dropdownData: {} as any,
	budgetTypes: [] as any[],
});

const rules = ref<FormRules>({
  budgetYear: [{required: true, message: '请选择预算年度！', trigger: 'blur',},],
  budgetName: [{required: true, message: '请选择预算名称！', trigger: 'blur',},],
  totalAmount: [{required: true, message: '请选择预算总额！', trigger: 'blur',},],
  executedAmount: [{required: true, message: '请选择已执行金额！', trigger: 'blur',},],
  remainAmount: [{required: true, message: '请选择剩余金额！', trigger: 'blur',},],
  status: [{required: true, message: '请选择状态！', trigger: 'change',},],
  version: [{required: true, message: '请选择版本号！', trigger: 'blur',},],
});

onMounted(async () => {
	await loadDropdownData();
});

const loadDropdownData = async () => {
	try {
		const typesRes = await finBudgetApi.getBudgetTypes();
		state.budgetTypes = typesRes.data.result || [];
	} catch (error) {
		console.error('加载下拉数据失败:', error);
	}
};

const openDialog = async (row: any, title: string) => {
	state.title = title;
	row = row ?? {};
	if (!row.id) {
		try {
			const res = await finBudgetApi.getNewBudgetNo();
			row.budgetNo = res.data.result;
		} catch (error) {
			console.error('获取预算编号失败:', error);
		}
	}
	state.ruleForm = row.id ? await finBudgetApi.detail(row.id).then(res => res.data.result) : JSON.parse(JSON.stringify(row));
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
			await finBudgetApi[state.ruleForm.id ? 'update' : 'add'](values);
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
	<div class="finBudget-container">
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
						<el-form-item label="预算编号" prop="budgetNo">
							<el-input v-model="state.ruleForm.budgetNo" placeholder="系统自动生成" maxlength="50" show-word-limit clearable disabled />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="预算年度" prop="budgetYear">
							<el-input-number v-model="state.ruleForm.budgetYear" placeholder="请输入预算年度" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="预算名称" prop="budgetName">
							<el-input v-model="state.ruleForm.budgetName" placeholder="请输入预算名称" maxlength="100" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="预算类型" prop="budgetType">
							<el-select v-model="state.ruleForm.budgetType" clearable filterable placeholder="请选择预算类型" style="width: 100%">
								<el-option v-for="item in state.budgetTypes" :key="item.code" :label="item.name" :value="item.code" />
							</el-select>
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="预算总额" prop="totalAmount">
							<el-input-number v-model="state.ruleForm.totalAmount" placeholder="请输入预算总额" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="已执行金额" prop="executedAmount">
							<el-input-number v-model="state.ruleForm.executedAmount" placeholder="请输入已执行金额" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="剩余金额" prop="remainAmount">
							<el-input-number v-model="state.ruleForm.remainAmount" placeholder="请输入剩余金额" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="状态" prop="status">
							<el-input v-model="state.ruleForm.status" placeholder="请输入状态" maxlength="20" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="版本号" prop="version">
							<el-input-number v-model="state.ruleForm.version" placeholder="请输入版本号" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="上级预算ID" prop="parentBudgetId">
							<el-input v-model="state.ruleForm.parentBudgetId" placeholder="请输入上级预算ID" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="备注" prop="remark">
							<el-input v-model="state.ruleForm.remark" placeholder="请输入备注" maxlength="500" show-word-limit clearable />
						</el-form-item>
					</el-col>
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