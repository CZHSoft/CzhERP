<script lang="ts" name="finAccountingPeriod" setup>
import { ref, reactive, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { formatDate } from '/@/utils/formatTime';
import { useFinAccountingPeriodApi } from '/@/api/finance/finAccountingPeriod';
//父级传递来的函数，用于回调
const emit = defineEmits(["reloadTable"]);
const finAccountingPeriodApi = useFinAccountingPeriodApi();
const ruleFormRef = ref();

const state = reactive({
	title: '',
	loading: false,
	showDialog: false,
	ruleForm: {} as any,
	stores: {},
	dropdownData: {} as any,
});

// 自行添加其他规则
const rules = ref<FormRules>({
  year: [{required: true, message: '请选择会计年度！', trigger: 'blur',},],
  period: [{required: true, message: '请选择期间序号！', trigger: 'blur',},],
  startDate: [{required: true, message: '请选择开始日期！', trigger: 'change',},],
  endDate: [{required: true, message: '请选择结束日期！', trigger: 'change',},],
  status: [{required: true, message: '请选择期间状态！', trigger: 'blur',},],
  isCurrent: [{required: true, message: '请选择是否当前期间！', trigger: 'blur',},],
  isClosed: [{required: true, message: '请选择是否已结账！', trigger: 'blur',},],
});

// 页面加载时
onMounted(async () => {
});

// 打开弹窗
const openDialog = async (row: any, title: string) => {
	state.title = title;
	row = row ?? {  };
	state.ruleForm = row.id ? await finAccountingPeriodApi.detail(row.id).then(res => res.data.result) : JSON.parse(JSON.stringify(row));
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
			await finAccountingPeriodApi[state.ruleForm.id ? 'update' : 'add'](values);
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
	<div class="finAccountingPeriod-container">
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
						<el-form-item label="会计年度" prop="year">
							<el-input-number v-model="state.ruleForm.year" placeholder="请输入会计年度" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="期间序号" prop="period">
							<el-input-number v-model="state.ruleForm.period" placeholder="请输入期间序号" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="开始日期" prop="startDate">
							<el-date-picker v-model="state.ruleForm.startDate" type="date" placeholder="开始日期" />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="结束日期" prop="endDate">
							<el-date-picker v-model="state.ruleForm.endDate" type="date" placeholder="结束日期" />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="期间状态" prop="status">
							<el-input v-model="state.ruleForm.status" placeholder="请输入期间状态" maxlength="20" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="是否当前期间" prop="isCurrent">
							<el-switch v-model="state.ruleForm.isCurrent" />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="是否已结账" prop="isClosed">
							<el-switch v-model="state.ruleForm.isClosed" />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="结账人ID" prop="closerUserId">
							<el-input v-model="state.ruleForm.closerUserId" placeholder="请输入结账人ID" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="结账时间" prop="closeTime">
							<el-date-picker v-model="state.ruleForm.closeTime" type="date" placeholder="结账时间" />
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