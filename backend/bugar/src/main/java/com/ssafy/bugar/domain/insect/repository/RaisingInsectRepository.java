package com.ssafy.bugar.domain.insect.repository;

import com.ssafy.bugar.domain.insect.entity.RaisingInsect;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface RaisingInsectRepository extends JpaRepository<RaisingInsect, Long> {
}
