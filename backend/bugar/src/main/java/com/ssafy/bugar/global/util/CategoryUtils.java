package com.ssafy.bugar.global.util;

import com.ssafy.bugar.domain.insect.enums.Category;

public class CategoryUtils {

    /**
     * 카테고리 번호로 카테고리 종류 반환
     * 1 -> FOOD(먹이) , 2 -> INTERACTION(쓰담) , 3 -> WEATHER(날씨)
     * @param categoryType
     * @return 카테고리 종류
     */
    public static Category getCategory(int categoryType) {
        if(categoryType == 1) {
            return Category.FOOD;
        } else if (categoryType == 2) {
            return Category.INTERACTION;
        } else if (categoryType == 3) {
            return Category.WEATHER;
        }

        throw new IllegalArgumentException("해당하는 카테고리를 찾을 수 없습니다.");
    }

    /**
     * 카테고리 종류에 따라 향상되는 애정도 점수 반환
     * @param category
     * @return 점수
     */
    public static int getCategoryScore(Category category) {
        if(category == Category.INTERACTION) {
            return 1;
        } else if (category == Category.FOOD) {
            return 3;
        } else if (category == Category.WEATHER) {
            return 5;
        }

        throw new IllegalArgumentException("해당하는 카테고리를 찾을 수 없습니다.");
    }

}
