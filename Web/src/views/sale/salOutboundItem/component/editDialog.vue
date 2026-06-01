<script lang="ts" name="salOutboundItem" setup>
import { ref, reactive, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { formatDate } from '/@/utils/formatTime';
import { useSalOutboundItemApi } from '/@/api/sale/salOutboundItem';
//父级传递来的函数，用于回调
const emit = defineEmits(["reloadTable"]);
const salOutboundItemApi = useSalOutboundItemApi();
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
  outboundId: [{required: true, message: '请选择出库单ID！', trigger: 'blur',},],
  materialId: [{required: true, message: '请选择物料ID！', trigger: 'blur',},],
  materialCode: [{required: true, message: '请选择物料编码！', trigger: 'blur',},],
  materialName: [{required: true, message: '请选择物料名称！', trigger: 'blur',},],
  unit: [{required: true, message: '请选择单位！', trigger: 'blur',},],
  orderQuantity: [{required: true, message: '请选择订单数量！', trigger: 'blur',},],
  outboundQuantity: [{required: true, message: '请选择出库数量！', trigger: 'blur',},],
  unitCost: [{required: true, message: '请选择单位成本！', trigger: 'blur',},],
  amount: [{required: true, message: '请选择金额！', trigger: 'blur',},],
  sortOrder: [{required: true, message: '请选择排序！', trigger: 'blur',},],
});

// 页面加载时
onMounted(async () => {
});

// 打开弹窗
const openDialog = async (row: any, title: string) => {
	state.title = title;
	row = row ?? {  };
	state.ruleForm = row.id ? await salOutboundItemApi.detail(row.id).then(res => res.data.result) : JSON.parse(JSON.stringify(row));
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
			await salOutboundItemApi[state.ruleForm.id ? 'update' : 'add'](values);
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
	<div class="salOutboundItem-container">
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
						<el-form-item label="出库单ID" prop="outboundId">
							<el-input v-model="state.ruleForm.outboundId" placeholder="请输入出库单ID" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="订单明细ID" prop="orderItemId">
							<el-input v-model="state.ruleForm.orderItemId" placeholder="请输入订单明细ID" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="物料ID" prop="materialId">
							<el-input v-model="state.ruleForm.materialId" placeholder="请输入物料ID" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="物料编码" prop="materialCode">
							<el-input v-model="state.ruleForm.materialCode" placeholder="请输入物料编码" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="物料名称" prop="materialName">
							<el-input v-model="state.ruleForm.materialName" placeholder="请输入物料名称" maxlength="100" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="规格型号" prop="spec">
							<el-input v-model="state.ruleForm.spec" placeholder="请输入规格型号" maxlength="100" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="单位" prop="unit">
							<el-input v-model="state.ruleForm.unit" placeholder="请输入单位" maxlength="20" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="订单数量" prop="orderQuantity">
							<el-input-number v-model="state.ruleForm.orderQuantity" placeholder="请输入订单数量" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="出库数量" prop="outboundQuantity">
							<el-input-number v-model="state.ruleForm.outboundQuantity" placeholder="请输入出库数量" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="库位编码" prop="locationCode">
							<el-input v-model="state.ruleForm.locationCode" placeholder="请输入库位编码" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="批次号" prop="batchNo">
							<el-input v-model="state.ruleForm.batchNo" placeholder="请输入批次号" maxlength="50" show-word-limit clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="有效期" prop="expiryDate">
							<el-date-picker v-model="state.ruleForm.expiryDate" type="date" placeholder="有效期" />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="单位成本" prop="unitCost">
							<el-input-number v-model="state.ruleForm.unitCost" placeholder="请输入单位成本" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="金额" prop="amount">
							<el-input-number v-model="state.ruleForm.amount" placeholder="请输入金额" clearable />
						</el-form-item>
					</el-col>
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20" >
						<el-form-item label="排序" prop="sortOrder">
							<el-input-number v-model="state.ruleForm.sortOrder" placeholder="请输入排序" clearable />
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