// 加入購物車的函數
function addToCart(dish) {
    // 從輸入框取得數量
    let quantityInput = document.getElementById(`quantity-${dish.DishID}`);
    let quantity = parseInt(quantityInput.value);

    if (quantity <= 0 || isNaN(quantity)) {
        alert("數量必須是有效的數字且大於 0");
        return;
    }

    let cart = JSON.parse(localStorage.getItem("cart")) || [];
    let existingItem = cart.find(item => item.DishID === dish.DishID);

    if (existingItem) {
        existingItem.Qty += quantity;
    } else {
        cart.push({
            DishID: dish.DishID,
            DishName: dish.DishName,
            DishPrice: dish.DishPrice,
            Qty: quantity
        });
    }

    localStorage.setItem("cart", JSON.stringify(cart));
    alert(`${dish.DishName} 已成功加入購物車！`);
    CartStatusCheck();
}

// 移除購物車中的項目
function removeItem(dishId) {
    if (confirm("確定要移除此商品嗎？")) {
        let cart = JSON.parse(localStorage.getItem("cart")) || [];
        // 使用 filter() 方法來移除指定商品
        let updatedCart = cart.filter(item => item.DishID !== dishId);

        localStorage.setItem("cart", JSON.stringify(updatedCart));
        CartStatusCheck();
        // 如果你的購物車頁面需要更新，這裡可以呼叫更新函數
        // refreshCartView(); 
    }
}

// 更新購物車圖示的函數
function CartStatusCheck() {
    let cart = JSON.parse(localStorage.getItem("cart")) || [];
    let totalCount = 0;

    if (cart.length > 0) {
        totalCount = cart.reduce((sum, item) => sum + item.Qty, 0);
    }

    const cartCountElement = document.getElementById("cart-item-count");
    if (cartCountElement) {
        cartCountElement.innerText = totalCount;
    }
}

// 頁面載入時，立即執行一次以確保購物車數量顯示正確
document.addEventListener("DOMContentLoaded", CartStatusCheck);