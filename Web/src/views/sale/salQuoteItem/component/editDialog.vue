<script lang="ts" name="salQuoteItem" setup>
import { ref, reactive, onMounted, watch } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { formatDate } from '/@/utils/formatTime';
import { useSalQuoteItemApi } from '/@/api/sale/salQuoteItem';

//父级传递来的函数，用于回调
const emit = defineEmits(["reloadTable"]);
const salQuoteItemApi = useSalQuoteItemApi();
const ruleFormRef = ref();

const state = reactive({
	title: '',
	loading: false,
	showDialog: false,
	ruleForm: {} as any,
	stores: {},
	dropdownData: {} as any,
});

// 报价单下拉选择数据
const quoteOptions = ref<Array<{
	value: number; 
	label: string; 
	quoteNo: string; 
	customerName: string; 
	quoteDate: string;
	totalAmount: number;
}>>([]);
const quoteLoading = ref(false);
const quoteSearchText = ref('');

// 物料下拉选择数据
const materialOptions = ref<Array<{
	value: number; 
	label: string; 
	materialCode: string; 
	spec: string;
	unit: string;
	salePrice: number;
	taxRate: number;
}>>([]);
const materialLoading = ref(false);
const materialSearchText = ref('');

// 自行添加其他规则
const rules = ref<FormRules>({
  quoteId: [{required: true, message: '请选择报价单！', trigger: 'change',},],
  materialId: [{required: true, message: '请选择物料！', trigger: 'change',},],
  materialCode: [{required: true, message: '物料编码不能为空！', trigger: 'blur',},],
  materialName: [{required: true, message: '物料名称不能为空！', trigger: 'blur',},],
  unit: [{required: true, message: '单位不能为空！', trigger: 'blur',},],
  quantity: [{required: true, message: '数量不能为空！', trigger: 'blur',},],
  unitPrice: [{required: true, message: '单价不能为空！', trigger: 'blur',},],
  taxRate: [{required: true, message: '税率不能为空！', trigger: 'blur',},],
  taxAmount: [{required: true, message: '税额不能为空！', trigger: 'blur',},],
  amount: [{required: true, message: '金额不能为空！', trigger: 'blur',},],
  discount: [{required: true, message: '折扣不能为空！', trigger: 'blur',},],
  sortOrder: [{required: true, message: '排序不能为空！', trigger: 'blur',},],
});

// 页面加载时
onMounted(async () => {
});

// 加载报价单列表
const loadQuoteList = async () => {
	if (quoteLoading.value) return;
	
	quoteLoading.value = true;
	try {
		const res = await salQuoteItemApi.getQuoteList({ keyword: quoteSearchText.value });
		if (res && res.data && res.data.result) {
			quoteOptions.value = res.data.result.map((item: any) => ({
				value: item.id,
				label: `${item.quoteNo} - ${item.customerName}`,
				quoteNo: item.quoteNo,
				customerName: item.customerName,
				quoteDate: item.quoteDate,
				totalAmount: item.totalAmount,
			}));
		} else {
			quoteOptions.value = [];
		}
	} catch (error) {
		console.error('获取报价单列表失败:', error);
		quoteOptions.value = [];
		ElMessage({
			message: '获取报价单列表失败，请稍后重试',
			type: "error",
		});
	} finally {
		quoteLoading.value = false;
	}
};

// 加载物料列表
const loadMaterialList = async () => {
	if (materialLoading.value) return;
	
	materialLoading.value = true;
	try {
		const res = await salQuoteItemApi.getMaterialList({ keyword: materialSearchText.value });
		if (res && res.data && res.data.result) {
			materialOptions.value = res.data.result.map((item: any) => ({
				value: item.id,
				label: item.materialName,
				materialCode: item.materialCode,
				spec: item.spec,
				unit: item.unit,
				salePrice: item.salePrice,
				taxRate: item.taxRate,
			}));
		} else {
			materialOptions.value = [];
		}
	} catch (error) {
		console.error('获取物料列表失败:', error);
		materialOptions.value = [];
		ElMessage({
			message: '获取物料列表失败，请稍后重试',
			type: "error",
		});
	} finally {
		materialLoading.value = false;
	}
};

// 报价单选择变化处理
const handleQuoteChange = (quoteId: number) => {
	if (!quoteId) {
		state.ruleForm.quoteId = '';
		return;
	}
	
	const selectedQuote = quoteOptions.value.find(item => item.value === quoteId);
	if (selectedQuote) {
		state.ruleForm.quoteId = selectedQuote.value;
	}
};

// 物料选择变化处理
const handleMaterialChange = async (materialId: number) => {
	if (!materialId) {
		state.ruleForm.materialId = '';
		state.ruleForm.materialCode = '';
		state.ruleForm.materialName = '';
		state.ruleForm.spec = '';
		state.ruleForm.unit = '';
		state.ruleForm.unitPrice = 0;
		state.ruleForm.taxRate = 0;
		return;
	}
	
	try {
		const res = await salQuoteItemApi.getMaterialDetail({ id: materialId });
		if (res && res.data && res.data.result) {
			const material = res.data.result;
			state.ruleForm.materialId = material.id;
			state.ruleForm.materialCode = material.materialCode;
			state.ruleForm.materialName = material.materialName;
			state.ruleForm.spec = material.spec;
			state.ruleForm.unit = material.unit;
			state.ruleForm.unitPrice = material.salePrice;
			state.ruleForm.taxRate = material.taxRate;
			
			// 自动计算金额和税额
			calculateAmounts();
		}
	} catch (error) {
		console.error('获取物料详情失败:', error);
		ElMessage({
			message: '获取物料详情失败，请稍后重试',
			type: "error",
		});
	}
};

// 搜索报价单
const handleQuoteSearch = (keyword: string) => {
	quoteSearchText.value = keyword;
};

// 搜索物料
const handleMaterialSearch = (keyword: string) => {
	materialSearchText.value = keyword;
};

// 下拉框打开时加载数据
const handleQuoteDropdownOpen = () => {
	if (quoteOptions.value.length === 0) {
		loadQuoteList();
	}
};

const handleMaterialDropdownOpen = () => {
	if (materialOptions.value.length === 0) {
		loadMaterialList();
	}
};

// 计算金额和税额
const calculateAmounts = () => {
	const quantity = state.ruleForm.quantity || 0;
	const unitPrice = state.ruleForm.unitPrice || 0;
	const discount = state.ruleForm.discount || 1;
	const taxRate = state.ruleForm.taxRate || 0;
	
	const amount = quantity * unitPrice * discount;
	const taxAmount = amount * (taxRate / 100);
	
	state.ruleForm.amount = amount;
	state.ruleForm.taxAmount = taxAmount;
};

// 监听数量、单价、折扣变化，自动计算金额
watch([
	() => state.ruleForm.quantity, 
	() => state.ruleForm.unitPrice, 
	() => state.ruleForm.discount,
	() => state.ruleForm.taxRate
], () => {
	calculateAmounts();
});

// 打开弹窗
const openDialog = async (row: any, title: string) => {
	state.title = title;
	row = row ?? {  };
	
	quoteOptions.value = [];
	materialOptions.value = [];
	
	if (row.id) {
		state.ruleForm = await salQuoteItemApi.detail(row.id).then(res => res.data.result);
	} else {
		state.ruleForm = {
			...row,
			quantity: 1,
			unitPrice: 0,
			discount: 1,
			taxRate: 13,
			taxAmount: 0,
			amount: 0,
			sortOrder: 0,
		};
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
			await salQuoteItemApi[state.ruleForm.id ? 'update' : 'add'](values);
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
	<div class="salQuoteItem-container">
		<el-dialog v-model="state.showDialog" :width="900" draggable :close-on-click-modal="false">
			<template #header>
				<div style="color: #fff">
					<span>{{ state.title }}</span>
				</div>
			</template>
			<el-form :model="state.ruleForm" ref="ruleFormRef" label-width="100px" :rules="rules">
				<el-row :gutter="35">
					<el-form-item v-show="false">
						<el-input v-model="state.ruleForm.id" />
					</el-form-item>
					
					<!-- 报价单选择 -->
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="报价单" prop="quoteId">
							<el-select 
								v-model="state.ruleForm.quoteId" 
								placeholder="请选择报价单" 
								filterable 
								remote
								:remote-method="loadQuoteList"
								:loading="quoteLoading"
								@change="handleQuoteChange"
								@visible-change="handleQuoteDropdownOpen"
								@clear="handleQuoteChange(null)"
								style="width: 100%"
							>
								<el-option 
									v-for="item in quoteOptions" 
									:key="item.value" 
									:label="item.label" 
									:value="item.value"
								>
									<div style="display: flex; flex-direction: column;">
										<span style="font-weight: 500;">{{ item.label }}</span>
										<span style="font-size: 12px; color: #999; margin-top: 2px;">
											日期: {{ formatDate(item.quoteDate) }} | 金额: {{ item.totalAmount?.toFixed(2) || '0.00' }}
										</span>
									</div>
								</el-option>
							</el-select>
						</el-form-item>
					</el-col>
					
					<!-- 物料选择 -->
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="物料名称" prop="materialId">
							<el-select 
								v-model="state.ruleForm.materialId" 
								placeholder="请选择物料" 
								filterable 
								remote
								:remote-method="loadMaterialList"
								:loading="materialLoading"
								@change="handleMaterialChange"
								@visible-change="handleMaterialDropdownOpen"
								@clear="handleMaterialChange(null)"
								style="width: 100%"
							>
								<el-option 
									v-for="item in materialOptions" 
									:key="item.value" 
									:label="item.label" 
									:value="item.value"
								>
									<div style="display: flex; flex-direction: column;">
										<span style="font-weight: 500;">{{ item.label }}</span>
										<span style="font-size: 12px; color: #999; margin-top: 2px;">
											编码: {{ item.materialCode }} | 规格: {{ item.spec || '无' }} | 单位: {{ item.unit }}
										</span>
									</div>
								</el-option>
							</el-select>
						</el-form-item>
					</el-col>
					
					<!-- 物料编码（只读） -->
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="物料编码" prop="materialCode">
							<el-input 
								v-model="state.ruleForm.materialCode" 
								placeholder="选择物料后自动填充" 
								maxlength="50" 
								show-word-limit 
								:disabled="true" 
							/>
						</el-form-item>
					</el-col>
					
					<!-- 物料ID（只读） -->
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="物料ID" prop="materialId">
							<el-input 
								v-model="state.ruleForm.materialId" 
								placeholder="选择物料后自动填充" 
								show-word-limit 
								:disabled="true" 
							/>
						</el-form-item>
					</el-col>
					
					<!-- 规格型号 -->
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="规格型号" prop="spec">
							<el-input 
								v-model="state.ruleForm.spec" 
								placeholder="请输入规格型号" 
								maxlength="100" 
								show-word-limit 
								clearable 
							/>
						</el-form-item>
					</el-col>
					
					<!-- 单位 -->
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="单位" prop="unit">
							<el-select v-model="state.ruleForm.unit" placeholder="请选择单位" style="width: 100%">
								<el-option label="个" value="个" />
								<el-option label="件" value="件" />
								<el-option label="台" value="台" />
								<el-option label="米" value="米" />
								<el-option label="公斤" value="公斤" />
								<el-option label="块" value="块" />
								<el-option label="套" value="套" />
								<el-option label="箱" value="箱" />
							</el-select>
						</el-form-item>
					</el-col>
					
					<!-- 数量 -->
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="数量" prop="quantity">
							<el-input-number 
								v-model="state.ruleForm.quantity" 
								placeholder="请输入数量" 
								:min="0" 
								:precision="2" 
								:step="1" 
								style="width: 100%"
							/>
						</el-form-item>
					</el-col>
					
					<!-- 单价 -->
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="单价" prop="unitPrice">
							<el-input-number 
								v-model="state.ruleForm.unitPrice" 
								placeholder="请输入单价" 
								:min="0" 
								:precision="2" 
								:step="1" 
								style="width: 100%"
							/>
						</el-form-item>
					</el-col>
					
					<!-- 折扣 -->
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="折扣" prop="discount">
							<el-input-number 
								v-model="state.ruleForm.discount" 
								placeholder="请输入折扣" 
								:min="0" 
								:max="1" 
								:precision="2" 
								:step="0.01" 
								style="width: 100%"
							/>
						</el-form-item>
					</el-col>
					
					<!-- 税率 -->
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="税率(%)" prop="taxRate">
							<el-input-number 
								v-model="state.ruleForm.taxRate" 
								placeholder="请输入税率" 
								:min="0" 
								:max="100" 
								:precision="2" 
								:step="1" 
								style="width: 100%"
							/>
						</el-form-item>
					</el-col>
					
					<!-- 税额（自动计算） -->
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="税额" prop="taxAmount">
							<el-input-number 
								v-model="state.ruleForm.taxAmount" 
								placeholder="自动计算" 
								:min="0" 
								:precision="2" 
								:disabled="true" 
								style="width: 100%"
							/>
						</el-form-item>
					</el-col>
					
					<!-- 金额（自动计算） -->
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="金额" prop="amount">
							<el-input-number 
								v-model="state.ruleForm.amount" 
								placeholder="自动计算" 
								:min="0" 
								:precision="2" 
								:disabled="true" 
								style="width: 100%"
							/>
						</el-form-item>
					</el-col>
					
					<!-- 排序 -->
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="排序" prop="sortOrder">
							<el-input-number 
								v-model="state.ruleForm.sortOrder" 
								placeholder="请输入排序" 
								:min="0" 
								:step="1" 
								style="width: 100%"
							/>
						</el-form-item>
					</el-col>
					
					<!-- 备注 -->
					<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" class="mb20">
						<el-form-item label="备注" prop="remark">
							<el-input 
								v-model="state.ruleForm.remark" 
								type="textarea" 
								placeholder="请输入备注" 
								maxlength="200" 
								show-word-limit 
								:rows="3"
							/>
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
