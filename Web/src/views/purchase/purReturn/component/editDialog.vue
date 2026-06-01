<script lang="ts" name="purReturn" setup>
import { ref, reactive, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { formatDate } from '/@/utils/formatTime';
import { usePurReturnApi } from '/@/api/purchase/purReturn';
//父级传递来的函数，用于回调
const emit = defineEmits(["reloadTable"]);
const purReturnApi = usePurReturnApi();
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
  returnNo: [{required: true, message: '请选择退货单号！', trigger: 'blur',},],
  inboundId: [{required: true, message: '请选择关联入库单ID！', trigger: 'blur',},],
  supplierId: [{required: true, message: '请选择供应商ID！', trigger: 'blur',},],
  supplierName: [{required: true, message: '请选择供应商名称！', trigger: 'blur',},],
  returnDate: [{required: true, message: '请选择退货日期！', trigger: 'change',},],
  totalQty: [{required: true, message: '请选择总数量！', trigger: 'blur',},],
  totalAmount: [{required: true, message: '请选择总金额！', trigger: 'blur',},],
  status: [{required: true, message: '请选择状态(0待审批/1已审批/2已出库/3已完成)！', trigger: 'blur',},],
});

// 页面加载时
onMounted(async () => {
});

// 打开弹窗
const openDialog = async (row: any, title: string) => {
	state.title = title;
	row = row ?? {  };
	state.ruleForm = row.id ? await purReturnApi.detail(row.id).then(res => res.data.result) : JSON.parse(JSON.stringify(row));
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
			await purReturnApi[state.ruleForm.id ? 'update' : 'add'](values);
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
	<div class="purReturn-container">
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
						<el-form-item label="退货单号" prop="returnNo">
							<el-input v-model="state.ruleForm.returnNo" placeholder="请输入退货单号" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="关联入库单ID" prop="inboundId">
							<el-input v-model="state.ruleForm.inboundId" placeholder="请输入关联入库单ID" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="供应商ID" prop="supplierId">
							<el-input v-model="state.ruleForm.supplierId" placeholder="请输入供应商ID" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="供应商名称" prop="supplierName">
							<el-input v-model="state.ruleForm.supplierName" placeholder="请输入供应商名称" maxlength="100" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="退货日期" prop="returnDate">
							<el-date-picker v-model="state.ruleForm.returnDate" type="date" placeholder="退货日期" />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="总数量" prop="totalQty">
							<el-input-number v-model="state.ruleForm.totalQty" placeholder="请输入总数量" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="总金额" prop="totalAmount">
							<el-input-number v-model="state.ruleForm.totalAmount" placeholder="请输入总金额" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="退货原因" prop="reason">
							<el-input v-model="state.ruleForm.reason" placeholder="请输入退货原因" maxlength="500" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="状态(0待审批/1已审批/2已出库/3已完成)" prop="status">
							<el-input-number v-model="state.ruleForm.status" placeholder="请输入状态(0待审批/1已审批/2已出库/3已完成)" clearable />
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