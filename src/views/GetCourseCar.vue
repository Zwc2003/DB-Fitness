<script setup>
// 假设的购物车数据
const cartStore = {
  isAll: false,
  cartList: [
    {
      id: 1,
      skuId: "sku001",
      selected: true,
      name: "课程A",
      price: 10,
      count: 1,
      picture:
        "https://ts1.cn.mm.bing.net/th/id/R-C.eb664ee18ebb1649333219359499c252?rik=%2becXG%2fRMM5w65A&riu=http%3a%2f%2ff1madness.co.za%2fwp-content%2fuploads%2f2015%2f08%2fKimi-Raikkonen_2015.jpg&ehk=eKJ%2fYd6CBv5qxX76v%2bgqYvnMauOn1nz3MJL1FEJaAY0%3d&risl=&pid=ImgRaw&r=0",
    },
    {
      id: 2,
      skuId: "sku002",
      selected: false,
      name: "课程B",
      price: 20,
      count: 2,
      picture: "@/assets/myimg.png",
    },
  ],
  allCount: 2,
  selectedCount: 1,
  selectedPrice: 300,
};

// 单选操作
const handleCheck = (skuId, selected) => {
  // 静态示例不执行任何操作
};

// 全选操作
const handleAllCheck = (selected) => {
  // 静态示例不执行任何操作
};

// 购物车中改变数量
const handeChangeCount = (skuId, count) => {
  // 静态示例不执行任何操作
};

// 删除购物车商品的示例函数
const deleteCart = (item) => {
  // 静态示例不执行任何操作
};

// import { useCartStore } from "@/stores";
// const cartStore = useCartStore();
</script>

<template>
  <div class="xtx-cart-page">
    <div class="container m-top-20">
      <div class="cart">
        <table>
          <thead>
            <tr>
              <th width="120">
                <el-checkbox
                  :modelValue="cartStore.isAll"
                  @change="handleAllCheck"
                />
              </th>
              <th width="400">课程名称</th>
              <th width="210">课程单价</th>
              <th width="210">课程时间</th>
              <th width="280">操作</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="i in cartStore.cartList" :key="i.id">
              <td>
                <el-checkbox
                  :modelValue="i.selected"
                  @change="(selected) => handleCheck(i.skuId, selected)"
                />
              </td>
              <td>
                <div class="goods">
                  <RouterLink to="/"
                    ><img
                      :src="
                        typeof i.picture === 'string' ? i.picture : i.picture[0]
                      "
                      alt=""
                  /></RouterLink>
                  <div>
                    <p class="name ellipsis">
                      {{ i.name }}
                    </p>
                  </div>
                </div>
              </td>
              <td class="tc">
                <p>&yen;{{ i.price }}</p>
              </td>
              <td class="tc">
                <el-input-number
                  :min="1"
                  :modelValue="i.count"
                  @input="(count) => handeChangeCount(i.skuId, count)"
                />
              </td>
              <td class="tc">
                <p class="f16 red">&yen;{{ (i.price * i.count).toFixed(2) }}</p>
              </td>
              <td class="tc">
                <p>
                  <el-popconfirm
                    title="确认删除吗?"
                    confirm-button-text="确认"
                    cancel-button-text="取消"
                    @confirm="deleteCart(i)"
                  >
                    <template #reference>
                      <a href="javascript:;">删除</a>
                    </template>
                  </el-popconfirm>
                </p>
              </td>
            </tr>
            <tr v-if="cartStore.cartList.length === 0">
              <td colspan="6">
                <div class="cart-none">
                  <el-empty description="购物车列表为空">
                    <el-button type="primary" @click="$router.push('/')"
                      >随便逛逛</el-button
                    >
                  </el-empty>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
      <!-- 操作栏 -->
      <div class="action">
        <div class="batch">
          共 {{ cartStore.allCount }} 件商品，已选择
          {{ cartStore.selectedCount }} 件，商品合计：
          <span class="red">¥ {{ cartStore.selectedPrice.toFixed(2) }} </span>
        </div>
        <div class="total">
          <el-button
            size="large"
            type="primary"
            @click="$router.push('/checkout')"
            >下单结算</el-button
          >
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped lang="scss">
.xtx-cart-page {
  margin-top: 20px;

  .cart {
    background: #fff;
    color: #666;

    table {
      border-spacing: 0;
      border-collapse: collapse;
      line-height: 24px;

      th,
      td {
        padding: 10px;
        border-bottom: 1px solid #f5f5f5;

        &:first-child {
          text-align: left;
          padding-left: 30px;
          color: #999;
        }
      }

      th {
        font-size: 16px;
        font-weight: normal;
        line-height: 50px;
      }
    }
  }

  .cart-none {
    text-align: center;
    padding: 120px 0;
    background: #fff;

    p {
      color: #999;
      padding: 20px 0;
    }
  }

  .tc {
    text-align: center;

    a {
      color: #27ba9b;
      //color: $xtxColor;
    }

    .xtx-numbox {
      margin: 0 auto;
      width: 120px;
    }
  }

  .red {
    color: #cf4444;
    //color: $priceColor;
  }

  .green {
    color: #27ba9b;
    //color: $xtxColor;
  }

  .f16 {
    font-size: 16px;
  }

  .goods {
    display: flex;
    align-items: center;

    img {
      width: 100px;
      height: 100px;
    }

    > div {
      width: 280px;
      font-size: 16px;
      padding-left: 10px;

      .attr {
        font-size: 14px;
        color: #999;
      }
    }
  }

  .action {
    display: flex;
    background: #fff;
    margin-top: 20px;
    height: 80px;
    align-items: center;
    font-size: 16px;
    justify-content: space-between;
    padding: 0 30px;

    .xtx-checkbox {
      color: #999;
    }

    .batch {
      a {
        margin-left: 20px;
      }
    }

    .red {
      font-size: 18px;
      margin-right: 20px;
      font-weight: bold;
    }
  }

  .tit {
    color: #666;
    font-size: 16px;
    font-weight: normal;
    line-height: 50px;
  }
}
</style>
