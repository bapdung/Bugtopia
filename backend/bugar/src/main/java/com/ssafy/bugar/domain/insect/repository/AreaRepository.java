package com.ssafy.bugar.domain.insect.repository;

import com.ssafy.bugar.domain.insect.entity.Area;
import org.springframework.data.jpa.repository.JpaRepository;

public interface AreaRepository extends JpaRepository<Area, Long> {

    Area findByAreaId(Long areaId);

}
