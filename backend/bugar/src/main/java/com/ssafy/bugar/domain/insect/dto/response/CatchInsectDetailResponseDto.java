package com.ssafy.bugar.domain.insect.dto.response;

import com.fasterxml.jackson.annotation.JsonInclude;
import java.util.List;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;

@Getter
@AllArgsConstructor
@JsonInclude(JsonInclude.Include.NON_NULL)
@Builder
public class CatchInsectDetailResponseDto {
    // 공통 속성
    private String krName;
    private String family;

    // 채집 곤충
    private String engName;
    private String imgUrl;
    private String info;
    private String photo;
    private Integer canRaise;
    private String area;
    private String rejectedReason;

    // 육성 완료 곤충
    private String insectNickname;
    private String startDate;
    private String doneDate;
    private Integer meetingDays;
    private int messageCnt;
    private List<MessageItem> messages;

    public interface CatchInsectDetailProjection {

        // 공통 속성
        String getKrName();

        String getFamily();

        // 채집 곤충
        String getEngName();

        String getInfo();

        String getPhoto();

        Integer getCanRaise();

        String getArea();

        String getRejectedReason();

        // 육성 완료 곤충
        String getInsectNickname();

        String getStartDate();

        String getDoneDate();

        Integer getMeetingDays();
    }

    public interface MessageItem {
        String getType();

        Long getNotificationId();

        boolean getIsRead();
    }
}
