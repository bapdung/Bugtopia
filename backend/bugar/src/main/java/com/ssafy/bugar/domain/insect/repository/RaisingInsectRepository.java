package com.ssafy.bugar.domain.insect.repository;

import com.ssafy.bugar.domain.insect.dto.response.GetAreaInsectResponseDto;
import com.ssafy.bugar.domain.insect.entity.RaisingInsect;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import java.util.List;

public interface RaisingInsectRepository extends JpaRepository<RaisingInsect, Long> {

    @Query(value = """
            SELECT ri.insect_id AS insectId, ri.insect_nickname AS nickname
            FROM raising_insects ri
            JOIN insects i ON ri.insect_id = i.insect_id
            JOIN area a ON i.area_id = a.area_id
            WHERE ri.user_id = :userId AND a.area_name = :areaName
            """, nativeQuery = true)
    List<GetAreaInsectResponseDto.InsectList> findInsectsByUserIdAndAreaName(@Param("userId") Long userId, @Param("areaName") String areaName);

}
