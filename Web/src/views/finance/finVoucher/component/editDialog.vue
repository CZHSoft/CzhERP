<script lang="ts" name="finVoucher" setup>
import { ref, reactive, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { formatDate } from '/@/utils/formatTime';
import { useFinVoucherApi } from '/@/api/finance/finVoucher';
//父级传递来的函数，用于回调
const emit = defineEmits(["reloadTable"]);
const finVoucherApi = useFinVoucherApi();
const ruleFormRef = ref();

const state = reactive({
	title: '',
	loading: false,
	showDialog: false,
	ruleForm: {} as any,
	stores: {},
	dropdownData: {} as any,
	statuses: [] as any[], // 状态下拉数据
});

// 自行添加其他规则
const rules = ref<FormRules>({
	voucherNo: [{required: true, message: '请选择凭证号！', trigger: 'blur',}],
	voucherDate: [{required: true, message: '请选择凭证日期！', trigger: 'change',}],
	year: [{required: true, message: '请选择会计年度！', trigger: 'blur',}],
	period: [{required: true, message: '请选择会计期间！', trigger: 'blur',}],
	attachmentCount: [{required: true, message: '请选择附件数量！', trigger: 'blur',}],
	totalDebit: [{required: true, message: '请选择借方金额合计！', trigger: 'blur',}],
	totalCredit: [{required: true, message: '请选择贷方金额合计！', trigger: 'blur',}],
	status: [{required: true, message: '请选择状态！', trigger: 'change',}],
});

// 页面加载时
onMounted(async () => {
	await loadDropdownData();
});

// 加载下拉数据
const loadDropdownData = async () => {
	try {
		const statusesRes = await finVoucherApi.getStatuses();
		state.statuses = statusesRes.data.result || [];
	} catch (error) {
		console.error('加载下拉数据失败:', error);
	}
};

// 打开弹窗
const openDialog = async (row: any, title: string) => {
	state.title = title;
	row = row ?? {  };
	if (!row.id) {
		// 新增时获取新凭证号
		try {
			const res = await finVoucherApi.getNewVoucherNo();
			row.voucherNo = res.data.result;
		} catch (error) {
			console.error('获取凭证号失败:', error);
		}
	}
	state.ruleForm = row.id ? await finVoucherApi.detail(row.id).then(res => res.data.result) : JSON.parse(JSON.stringify(row));
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
			await finVoucherApi[state.ruleForm.id ? 'update' : 'add'](values);
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
	<div class="finVoucher-container">
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
						<el-form-item label="凭证号" prop="voucherNo">
							<el-input v-model="state.ruleForm.voucherNo" placeholder="请输入凭证号" maxlength="50" show-word-limit clearable disabled />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="凭证字" prop="voucherWord">
							<el-input v-model="state.ruleForm.voucherWord" placeholder="请输入凭证字" maxlength="20" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="凭证日期" prop="voucherDate">
							<el-date-picker v-model="state.ruleForm.voucherDate" type="date" placeholder="凭证日期" />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="会计年度" prop="year">
							<el-input-number v-model="state.ruleForm.year" placeholder="请输入会计年度" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="会计期间" prop="period">
							<el-input-number v-model="state.ruleForm.period" placeholder="请输入会计期间" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="附件数量" prop="attachmentCount">
							<el-input-number v-model="state.ruleForm.attachmentCount" placeholder="请输入附件数量" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="来源单据类型" prop="sourceType">
							<el-input v-model="state.ruleForm.sourceType" placeholder="请输入来源单据类型" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="来源单据ID" prop="sourceId">
							<el-input v-model="state.ruleForm.sourceId" placeholder="请输入来源单据ID" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="来源单据号" prop="sourceNo">
							<el-input v-model="state.ruleForm.sourceNo" placeholder="请输入来源单据号" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="借方金额合计" prop="totalDebit">
							<el-input-number v-model="state.ruleForm.totalDebit" placeholder="请输入借方金额合计" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="贷方金额合计" prop="totalCredit">
							<el-input-number v-model="state.ruleForm.totalCredit" placeholder="请输入贷方金额合计" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="状态" prop="status">
							<el-select v-model="state.ruleForm.status" clearable filterable placeholder="请选择状态" style="width: 100%">
								<el-option v-for="item in state.statuses" :key="item.code" :label="item.name" :value="item.code" />
							</el-select>
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="制单人ID" prop="makerId">
							<el-input v-model="state.ruleForm.makerId" placeholder="请输入制单人ID" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="制单人姓名" prop="makerName">
							<el-input v-model="state.ruleForm.makerName" placeholder="请输入制单人姓名" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="制单时间" prop="makeTime">
							<el-date-picker v-model="state.ruleForm.makeTime" type="date" placeholder="制单时间" />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="审核人ID" prop="approverId">
							<el-input v-model="state.ruleForm.approverId" placeholder="请输入审核人ID" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="审核人姓名" prop="approverName">
							<el-input v-model="state.ruleForm.approverName" placeholder="请输入审核人姓名" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="审核时间" prop="approveTime">
							<el-date-picker v-model="state.ruleForm.approveTime" type="date" placeholder="审核时间" />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="过账人ID" prop="posterId">
							<el-input v-model="state.ruleForm.posterId" placeholder="请输入过账人ID" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="过账人姓名" prop="posterName">
							<el-input v-model="state.ruleForm.posterName" placeholder="请输入过账人姓名" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="过账时间" prop="postTime">
							<el-date-picker v-model="state.ruleForm.postTime" type="date" placeholder="过账时间" />
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